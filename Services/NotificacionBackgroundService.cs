using ActividadApp.Data;
using ActividadApp.Models;
using ActividadApp.Services.Email;
using Microsoft.EntityFrameworkCore;

namespace ActividadApp.Services;

public class NotificacionBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NotificacionBackgroundService> _logger;
    private static readonly TimeSpan Intervalo = TimeSpan.FromHours(1);

    public NotificacionBackgroundService(IServiceScopeFactory scopeFactory, ILogger<NotificacionBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NotificacionBackgroundService iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcesarNotificaciones(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar notificaciones.");
            }

            await Task.Delay(Intervalo, stoppingToken);
        }
    }

    private async Task ProcesarNotificaciones(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        var acciones = await context.Acciones
            .Include(a => a.Proceso)
                .ThenInclude(p => p.Responsable)
            .Include(a => a.Solucion)
                .ThenInclude(s => s.Actividades)
                    .ThenInclude(act => act.Responsable)
            .Include(a => a.Solucion)
                .ThenInclude(s => s.UsuarioInvestiga)
            .Include(a => a.ConfirmacionPlanAccion)
            .Include(a => a.Eficacia)
            .Include(a => a.Estado)
            .Where(a => a.EstadoId >= 1 && a.EstadoId <= 4)
            .ToListAsync(ct);

        var admins = await context.Users
            .Where(u => u.RolId == 2)
            .ToListAsync(ct);

        var ahora = DateTime.UtcNow;
        var accionesActualizadas = new List<Accion>();

        foreach (var accion in acciones)
        {
            var resultado = EvaluarNotificacion(accion, ahora);
            if (resultado == null) continue;

            var consecutivo = $"ACC-{accion.Id:D4}";
            var proceso = accion.Proceso?.Descripcion ?? "-";
            var estado = accion.Estado?.Descripcion ?? "-";

            var enviado = false;

            switch (accion.EstadoId)
            {
                case 1: // Abierta sin Plan -> notificar al responsable del proceso
                    if (accion.Proceso?.Responsable != null)
                    {
                        enviado = await emailService.EnviarRecordatorio(
                            accion.Proceso.Responsable.Email,
                            accion.Proceso.Responsable.Nombre,
                            consecutivo, proceso, accion.Descripcion,
                            estado, resultado.Value);
                    }
                    break;

                case 2: // Abierta con Solución -> notificar a responsables de actividades pendientes
                    if (accion.Solucion?.Actividades != null)
                    {
                        var pendientes = accion.Solucion.Actividades
                            .Where(a => !a.Ejecutada && a.Responsable != null)
                            .ToList();

                        foreach (var act in pendientes)
                        {
                            var diasActividad = (int)(ahora - accion.Solucion.FechaCompromiso).TotalDays;
                            var envioActividad = await emailService.EnviarRecordatorio(
                                act.Responsable.Email,
                                act.Responsable.Nombre,
                                consecutivo, proceso, act.Descripcion,
                                estado, Math.Abs(diasActividad));
                            if (envioActividad) enviado = true;
                        }
                    }
                    break;

                case 3: // Abierta por Confirmación -> notificar al investigador
                    if (accion.Solucion?.UsuarioInvestiga != null)
                    {
                        enviado = await emailService.EnviarRecordatorio(
                            accion.Solucion.UsuarioInvestiga.Email,
                            accion.Solucion.UsuarioInvestiga.Nombre,
                            consecutivo, proceso, accion.Descripcion,
                            estado, resultado.Value);
                    }
                    break;

                case 4: // Pendiente por Eficacia -> notificar a administradores
                    foreach (var admin in admins)
                    {
                        enviado = await emailService.EnviarRecordatorio(
                            admin.Email, admin.Nombre,
                            consecutivo, proceso, accion.Descripcion,
                            estado, resultado.Value);
                    }
                    break;
            }

            if (enviado)
            {
                accion.UltimaNotificacion = ahora;
                accionesActualizadas.Add(accion);
                _logger.LogInformation("Recordatorio enviado para {Consecutivo} (Estado {Estado})", consecutivo, estado);
            }
        }

        if (accionesActualizadas.Any())
        {
            await context.SaveChangesAsync(ct);
        }
    }

    /// <summary>
    /// Evalúa si una acción necesita notificación. Retorna los días pendiente si debe notificar, null si no.
    /// </summary>
    private static int? EvaluarNotificacion(Accion accion, DateTime ahora)
    {
        return accion.EstadoId switch
        {
            1 => EvaluarEstado1(accion, ahora),
            2 => EvaluarEstado2(accion, ahora),
            3 => EvaluarEstado3(accion, ahora),
            4 => EvaluarEstado4(accion, ahora),
            _ => null
        };
    }

    // Estado 1: Abierta sin Plan
    // Primer recordatorio a los 10 días, luego cada 8 días
    private static int? EvaluarEstado1(Accion accion, DateTime ahora)
    {
        var diasDesdeCreacion = (int)(ahora - accion.CreatedAt).TotalDays;
        if (diasDesdeCreacion < 10) return null;

        if (accion.UltimaNotificacion == null)
            return diasDesdeCreacion;

        var diasDesdeUltima = (int)(ahora - accion.UltimaNotificacion.Value).TotalDays;
        return diasDesdeUltima >= 8 ? diasDesdeCreacion : null;
    }

    // Estado 2: Abierta con Solución
    // Recordatorio 5 días antes del vencimiento, luego cada 8 días después de vencer
    private static int? EvaluarEstado2(Accion accion, DateTime ahora)
    {
        if (accion.Solucion == null) return null;

        var fechaCompromiso = accion.Solucion.FechaCompromiso;
        var diasParaVencer = (int)(fechaCompromiso - ahora).TotalDays;
        var diasVencido = (int)(ahora - fechaCompromiso).TotalDays;

        // Aún no es momento de notificar (faltan más de 5 días)
        if (diasParaVencer > 5) return null;

        if (accion.UltimaNotificacion == null)
            return diasParaVencer > 0 ? diasParaVencer : diasVencido;

        var diasDesdeUltima = (int)(ahora - accion.UltimaNotificacion.Value).TotalDays;

        if (diasParaVencer > 0)
        {
            // Antes del vencimiento: solo una notificación
            return null;
        }

        // Después del vencimiento: cada 8 días
        return diasDesdeUltima >= 8 ? diasVencido : null;
    }

    // Estado 3: Abierta por Confirmación
    // Primer recordatorio a los 8 días, luego cada 8 días
    private static int? EvaluarEstado3(Accion accion, DateTime ahora)
    {
        var fechaReferencia = accion.UpdatedAt != default ? accion.UpdatedAt : accion.CreatedAt;
        var diasDesdeActualizacion = (int)(ahora - fechaReferencia).TotalDays;

        if (diasDesdeActualizacion < 8) return null;

        if (accion.UltimaNotificacion == null)
            return diasDesdeActualizacion;

        var diasDesdeUltima = (int)(ahora - accion.UltimaNotificacion.Value).TotalDays;
        return diasDesdeUltima >= 8 ? diasDesdeActualizacion : null;
    }

    // Estado 4: Pendiente por Eficacia
    // Primer recordatorio según DiasParaVerificar del Seguimiento SGI, luego cada 15 días
    private static int? EvaluarEstado4(Accion accion, DateTime ahora)
    {
        var fechaReferencia = accion.UpdatedAt != default ? accion.UpdatedAt : accion.CreatedAt;
        var diasDesdeConfirmacion = (int)(ahora - fechaReferencia).TotalDays;

        var diasParaVerificar = accion.ConfirmacionPlanAccion != null ? 90 : 90; // Obtener del último SeguimientoSgi si existe
        if (diasDesdeConfirmacion < diasParaVerificar) return null;

        if (accion.UltimaNotificacion == null)
            return diasDesdeConfirmacion;

        var diasDesdeUltima = (int)(ahora - accion.UltimaNotificacion.Value).TotalDays;
        return diasDesdeUltima >= 15 ? diasDesdeConfirmacion : null;
    }
}
