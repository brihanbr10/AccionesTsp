using ActividadApp.Data;
using ActividadApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ActividadApp.Services;

public class AccionService
{
    private readonly AppDbContext _context;
    private readonly ILogger<AccionService> _logger;

    public AccionService(AppDbContext context, ILogger<AccionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Accion>> GetAll()
    {
        try
        {
            return await _context.Acciones
                .Include(a => a.Usuario)
                .Include(a => a.Proceso)
                .Include(a => a.Solucion)
                    .ThenInclude(s => s.Actividades)
                        .ThenInclude(act => act.Seguimientos)
                            .ThenInclude(seg => seg.Usuario)
                .Include(a => a.Solucion)
                    .ThenInclude(s => s.UsuarioInvestiga)
                .Include(a => a.Solucion)
                    .ThenInclude(s => s.UsuarioCoordina)
                .Include(a => a.ConfirmacionPlanAccion)
                    .ThenInclude(c => c.Responsable)
                .Include(a => a.Eficacia)
                .Include(a => a.Estado)
                .Include(a => a.SeguimientosSgi)
                    .ThenInclude(s => s.Usuario)
                .OrderByDescending(a => a.Id)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las acciones");
            return new List<Accion>();
        }
    }

    public async Task<Accion?> GetById(int id)
    {
        try
        {
            return await _context.Acciones
                .Include(a => a.Usuario)
                .Include(a => a.Proceso)
                .Include(a => a.Solucion)
                    .ThenInclude(s => s.Actividades)
                        .ThenInclude(act => act.Seguimientos)
                            .ThenInclude(seg => seg.Usuario)
                .Include(a => a.Solucion)
                    .ThenInclude(s => s.UsuarioInvestiga)
                .Include(a => a.Solucion)
                    .ThenInclude(s => s.UsuarioCoordina)
                .Include(a => a.ConfirmacionPlanAccion)
                    .ThenInclude(c => c.Responsable)
                .Include(a => a.Eficacia)
                .Include(a => a.Estado)
                .Include(a => a.SeguimientosSgi)
                    .ThenInclude(s => s.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la accion {Id}", id);
            return null;
        }
    }

    public static int CalcularEstadoId(Accion accion)
    {
        if (accion.Eficacia != null) return 5;                    // Cerrada
        if (accion.ConfirmacionPlanAccion != null) return 4;      // Pendiente por Eficacia
        if (accion.Solucion != null
            && accion.Solucion.Actividades != null
            && accion.Solucion.Actividades.Any()
            && accion.Solucion.Actividades.All(a => a.Ejecutada)) return 3; // Abierta por Confirmación
        if (accion.Solucion != null) return 2;                    // Abierta con Solución
        return 1;                                                 // Abierta sin Plan
    }

    public async Task<bool> Create(Accion actividad)
    {
        try
        {
            actividad.CreatedAt = DateTime.UtcNow;
            _context.Acciones.Add(actividad);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la accion");
            return false;
        }
    }

    public async Task<bool> Update(Accion actividad)
    {
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la accion {Id}", actividad.Id);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var actividad = await _context.Acciones.FindAsync(id);
            if (actividad == null)
                return false;

            _context.Acciones.Remove(actividad);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la accion {Id}", id);
            return false;
        }
    }
}

