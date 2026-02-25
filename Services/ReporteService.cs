using ActividadApp.Data;
using ActividadApp.Models;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;

namespace ActividadApp.Services;

public class ReporteService
{
    private readonly AppDbContext _context;

    public ReporteService(AppDbContext context)
    {
        _context = context;
    }

    private async Task<List<Accion>> ObtenerAcciones(DateTime fechaDesde, DateTime fechaHasta, int? procesoId = null)
    {
        var query = _context.Acciones
            .Include(a => a.Usuario)
            .Include(a => a.Proceso)
                .ThenInclude(p => p.Responsable)
            .Include(a => a.Solucion)
                .ThenInclude(s => s.Actividades)
                    .ThenInclude(act => act.Responsable)
            .Include(a => a.Solucion)
                .ThenInclude(s => s.UsuarioInvestiga)
            .Include(a => a.Solucion)
                .ThenInclude(s => s.UsuarioCoordina)
            .Include(a => a.ConfirmacionPlanAccion)
                .ThenInclude(c => c.Responsable)
            .Include(a => a.Eficacia)
            .Include(a => a.Estado)
            .Where(a => a.Fecha >= fechaDesde && a.Fecha <= fechaHasta);

        if (procesoId.HasValue && procesoId.Value > 0)
            query = query.Where(a => a.ProcesoId == procesoId.Value);

        return await query.OrderBy(a => a.Id).ToListAsync();
    }

    private string TipoAccionTexto(int tipo) => tipo switch
    {
        1 => "Preventiva",
        2 => "Correctiva",
        3 => "Mejora",
        _ => "-"
    };

    private async Task<Dictionary<int, string>> ObtenerMaestros()
    {
        return await _context.Maestros.ToDictionaryAsync(m => m.Id, m => m.Descripcion);
    }

    // ??? REPORTE GENERAL ?????????????????????????????????????????????
    public async Task<byte[]> GenerarReporteGeneral(DateTime fechaDesde, DateTime fechaHasta)
    {
        var acciones = await ObtenerAcciones(fechaDesde, fechaHasta);
        var maestros = await ObtenerMaestros();

        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Reporte General");

        // Encabezados
        var headers = new[]
        {
            "Consecutivo", "Fecha", "Tipo Accion", "Proceso", "Responsable Proceso",
            "Descripcion", "Reporta", "Origen", "Entidad", "Sistema Gestion",
            "Organizacion", "Sitio", "Estado",
            "Correccion Propuesta", "Analisis Causa", "Investigador", "Fecha Investigacion",
            "Coordinador", "Fecha Compromiso",
            "Confirmacion", "Fecha Confirmacion", "Cumplio Plan",
            "Comentarios Eficacia", "Fecha Eficacia", "Eficaz"
        };

        for (int c = 0; c < headers.Length; c++)
            ws.Cell(1, c + 1).Value = headers[c];

        EstiloEncabezado(ws, 1, headers.Length);

        // Datos
        int fila = 2;
        foreach (var a in acciones)
        {
            int col = 1;
            ws.Cell(fila, col++).Value = $"ACC-{a.Id:D4}";
            ws.Cell(fila, col++).Value = a.Fecha.ToString("dd/MM/yyyy");
            ws.Cell(fila, col++).Value = TipoAccionTexto(a.TipoAccionId);
            ws.Cell(fila, col++).Value = a.Proceso?.Descripcion ?? "-";
            ws.Cell(fila, col++).Value = a.Proceso?.Responsable?.Nombre ?? "-";
            ws.Cell(fila, col++).Value = a.Descripcion;
            ws.Cell(fila, col++).Value = a.Usuario?.Nombre ?? "-";
            ws.Cell(fila, col++).Value = maestros.GetValueOrDefault(a.OrigenId, "-");
            ws.Cell(fila, col++).Value = maestros.GetValueOrDefault(a.EntidadId, "-");
            ws.Cell(fila, col++).Value = maestros.GetValueOrDefault(a.SistemaGestionId, "-");
            ws.Cell(fila, col++).Value = maestros.GetValueOrDefault(a.OrganizacionId, "-");
            ws.Cell(fila, col++).Value = maestros.GetValueOrDefault(a.SitioId, "-");
            ws.Cell(fila, col++).Value = a.Estado?.Descripcion ?? "-";

            // Solucion
            ws.Cell(fila, col++).Value = a.Solucion?.CorreccionPropuesta ?? "";
            ws.Cell(fila, col++).Value = a.Solucion?.AnalisisCausa ?? "";
            ws.Cell(fila, col++).Value = a.Solucion?.UsuarioInvestiga?.Nombre ?? "";
            ws.Cell(fila, col++).Value = a.Solucion?.FechaInvestigacion.ToString("dd/MM/yyyy") ?? "";
            ws.Cell(fila, col++).Value = a.Solucion?.UsuarioCoordina?.Nombre ?? "";
            ws.Cell(fila, col++).Value = a.Solucion?.FechaCompromiso.ToString("dd/MM/yyyy") ?? "";

            // Confirmacion
            ws.Cell(fila, col++).Value = a.ConfirmacionPlanAccion?.DetallesCumplimiento ?? "";
            ws.Cell(fila, col++).Value = a.ConfirmacionPlanAccion?.Fecha.ToString("dd/MM/yyyy") ?? "";
            ws.Cell(fila, col++).Value = a.ConfirmacionPlanAccion != null ? (a.ConfirmacionPlanAccion.CumplioPlan ? "Si" : "No") : "";

            // Eficacia
            ws.Cell(fila, col++).Value = a.Eficacia?.Comentarios ?? "";
            ws.Cell(fila, col++).Value = a.Eficacia?.Fecha.ToString("dd/MM/yyyy") ?? "";
            ws.Cell(fila, col++).Value = a.Eficacia != null ? (a.Eficacia.Eficaz ? "Eficaz" : "No Eficaz") : "";

            fila++;
        }

        ws.Columns().AdjustToContents();
        AgregarHojaResumen(workbook, acciones, fechaDesde, fechaHasta);

        return WorkbookToBytes(workbook);
    }

    // ??? REPORTE DETALLADO ???????????????????????????????????????????
    public async Task<byte[]> GenerarReporteDetallado(DateTime fechaDesde, DateTime fechaHasta)
    {
        var acciones = await ObtenerAcciones(fechaDesde, fechaHasta);
        var maestros = await ObtenerMaestros();

        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Reporte Detallado");

        var headers = new[]
        {
            "Consecutivo", "Fecha", "Tipo Accion", "Proceso", "Responsable Proceso",
            "Descripcion", "Reporta", "Origen", "Entidad", "Sistema Gestion",
            "Organizacion", "Sitio", "Estado",
            "Correccion Propuesta", "Analisis Causa", "Investigador", "Fecha Investigacion",
            "Coordinador", "Fecha Compromiso",
            "Actividad", "Agencia", "Responsable Actividad", "Fecha Actividad",
            "Ejecutada", "Fecha Ejecucion", "Observaciones",
            "Confirmacion", "Fecha Confirmacion", "Cumplio Plan",
            "Comentarios Eficacia", "Fecha Eficacia", "Eficaz"
        };

        for (int c = 0; c < headers.Length; c++)
            ws.Cell(1, c + 1).Value = headers[c];

        EstiloEncabezado(ws, 1, headers.Length);

        int fila = 2;
        foreach (var a in acciones)
        {
            var actividades = a.Solucion?.Actividades ?? new List<Actividad>();
            int filasPorAccion = Math.Max(1, actividades.Count);

            for (int i = 0; i < filasPorAccion; i++)
            {
                int col = 1;

                // Info general (solo en primera fila de la accion)
                if (i == 0)
                {
                    ws.Cell(fila, col).Value = $"ACC-{a.Id:D4}";
                    ws.Cell(fila, col + 1).Value = a.Fecha.ToString("dd/MM/yyyy");
                    ws.Cell(fila, col + 2).Value = TipoAccionTexto(a.TipoAccionId);
                    ws.Cell(fila, col + 3).Value = a.Proceso?.Descripcion ?? "-";
                    ws.Cell(fila, col + 4).Value = a.Proceso?.Responsable?.Nombre ?? "-";
                    ws.Cell(fila, col + 5).Value = a.Descripcion;
                    ws.Cell(fila, col + 6).Value = a.Usuario?.Nombre ?? "-";
                    ws.Cell(fila, col + 7).Value = maestros.GetValueOrDefault(a.OrigenId, "-");
                    ws.Cell(fila, col + 8).Value = maestros.GetValueOrDefault(a.EntidadId, "-");
                    ws.Cell(fila, col + 9).Value = maestros.GetValueOrDefault(a.SistemaGestionId, "-");
                    ws.Cell(fila, col + 10).Value = maestros.GetValueOrDefault(a.OrganizacionId, "-");
                    ws.Cell(fila, col + 11).Value = maestros.GetValueOrDefault(a.SitioId, "-");
                    ws.Cell(fila, col + 12).Value = a.Estado?.Descripcion ?? "-";

                    ws.Cell(fila, col + 13).Value = a.Solucion?.CorreccionPropuesta ?? "";
                    ws.Cell(fila, col + 14).Value = a.Solucion?.AnalisisCausa ?? "";
                    ws.Cell(fila, col + 15).Value = a.Solucion?.UsuarioInvestiga?.Nombre ?? "";
                    ws.Cell(fila, col + 16).Value = a.Solucion?.FechaInvestigacion.ToString("dd/MM/yyyy") ?? "";
                    ws.Cell(fila, col + 17).Value = a.Solucion?.UsuarioCoordina?.Nombre ?? "";
                    ws.Cell(fila, col + 18).Value = a.Solucion?.FechaCompromiso.ToString("dd/MM/yyyy") ?? "";
                }

                col = 20; // Columna de actividades

                if (i < actividades.Count)
                {
                    var act = actividades[i];
                    ws.Cell(fila, col++).Value = act.Descripcion;
                    ws.Cell(fila, col++).Value = act.Agencia;
                    ws.Cell(fila, col++).Value = act.Responsable?.Nombre ?? "-";
                    ws.Cell(fila, col++).Value = act.Fecha.ToString("dd/MM/yyyy");
                    ws.Cell(fila, col++).Value = act.Ejecutada ? "Si" : "No";
                    ws.Cell(fila, col++).Value = act.FechaEjecucion?.ToString("dd/MM/yyyy") ?? "";
                    ws.Cell(fila, col++).Value = act.Observaciones ?? "";
                }
                else
                {
                    col += 7;
                }

                // Confirmacion y eficacia (solo en primera fila)
                if (i == 0)
                {
                    ws.Cell(fila, col++).Value = a.ConfirmacionPlanAccion?.DetallesCumplimiento ?? "";
                    ws.Cell(fila, col++).Value = a.ConfirmacionPlanAccion?.Fecha.ToString("dd/MM/yyyy") ?? "";
                    ws.Cell(fila, col++).Value = a.ConfirmacionPlanAccion != null ? (a.ConfirmacionPlanAccion.CumplioPlan ? "Si" : "No") : "";
                    ws.Cell(fila, col++).Value = a.Eficacia?.Comentarios ?? "";
                    ws.Cell(fila, col++).Value = a.Eficacia?.Fecha.ToString("dd/MM/yyyy") ?? "";
                    ws.Cell(fila, col++).Value = a.Eficacia != null ? (a.Eficacia.Eficaz ? "Eficaz" : "No Eficaz") : "";
                }

                fila++;
            }

            // Merge celdas de info general si hay multiples actividades
            if (filasPorAccion > 1)
            {
                int filaInicio = fila - filasPorAccion;
                for (int c = 1; c <= 19; c++)
                {
                    ws.Range(filaInicio, c, fila - 1, c).Merge();
                    ws.Cell(filaInicio, c).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                }
                // Merge confirmacion y eficacia
                for (int c = 27; c <= 32; c++)
                {
                    ws.Range(filaInicio, c, fila - 1, c).Merge();
                    ws.Cell(filaInicio, c).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                }
            }
        }

        ws.Columns().AdjustToContents();
        AgregarHojaResumen(workbook, acciones, fechaDesde, fechaHasta);

        return WorkbookToBytes(workbook);
    }

    // ??? REPORTE POR PROCESO ?????????????????????????????????????????
    public async Task<byte[]> GenerarReportePorProceso(DateTime fechaDesde, DateTime fechaHasta, int procesoId)
    {
        var acciones = await ObtenerAcciones(fechaDesde, fechaHasta, procesoId);
        var maestros = await ObtenerMaestros();
        var proceso = await _context.Procesos.Include(p => p.Responsable).FirstOrDefaultAsync(p => p.Id == procesoId);

        using var workbook = new XLWorkbook();
        var nombreProceso = proceso?.Descripcion ?? "Proceso";
        var ws = workbook.Worksheets.Add("Reporte Proceso");

        // Titulo
        ws.Cell(1, 1).Value = $"Proceso: {nombreProceso}";
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 14;
        ws.Range(1, 1, 1, 6).Merge();

        ws.Cell(2, 1).Value = $"Responsable: {proceso?.Responsable?.Nombre ?? "-"}";
        ws.Cell(3, 1).Value = $"Periodo: {fechaDesde:dd/MM/yyyy} - {fechaHasta:dd/MM/yyyy}";

        var headers = new[]
        {
            "Consecutivo", "Fecha", "Tipo Accion", "Descripcion", "Reporta",
            "Origen", "Entidad", "Sistema Gestion", "Organizacion", "Sitio",
            "Estado", "Fecha Compromiso",
            "Cumplio Plan", "Eficaz"
        };

        int filaHeader = 5;
        for (int c = 0; c < headers.Length; c++)
            ws.Cell(filaHeader, c + 1).Value = headers[c];

        EstiloEncabezado(ws, filaHeader, headers.Length);

        int fila = filaHeader + 1;
        foreach (var a in acciones)
        {
            int col = 1;
            ws.Cell(fila, col++).Value = $"ACC-{a.Id:D4}";
            ws.Cell(fila, col++).Value = a.Fecha.ToString("dd/MM/yyyy");
            ws.Cell(fila, col++).Value = TipoAccionTexto(a.TipoAccionId);
            ws.Cell(fila, col++).Value = a.Descripcion;
            ws.Cell(fila, col++).Value = a.Usuario?.Nombre ?? "-";
            ws.Cell(fila, col++).Value = maestros.GetValueOrDefault(a.OrigenId, "-");
            ws.Cell(fila, col++).Value = maestros.GetValueOrDefault(a.EntidadId, "-");
            ws.Cell(fila, col++).Value = maestros.GetValueOrDefault(a.SistemaGestionId, "-");
            ws.Cell(fila, col++).Value = maestros.GetValueOrDefault(a.OrganizacionId, "-");
            ws.Cell(fila, col++).Value = maestros.GetValueOrDefault(a.SitioId, "-");
            ws.Cell(fila, col++).Value = a.Estado?.Descripcion ?? "-";
            ws.Cell(fila, col++).Value = a.Solucion?.FechaCompromiso.ToString("dd/MM/yyyy") ?? "";
            ws.Cell(fila, col++).Value = a.ConfirmacionPlanAccion != null ? (a.ConfirmacionPlanAccion.CumplioPlan ? "Si" : "No") : "";
            ws.Cell(fila, col++).Value = a.Eficacia != null ? (a.Eficacia.Eficaz ? "Eficaz" : "No Eficaz") : "";
            fila++;
        }

        ws.Columns().AdjustToContents();
        AgregarHojaResumen(workbook, acciones, fechaDesde, fechaHasta);

        return WorkbookToBytes(workbook);
    }

    // ??? UTILIDADES ??????????????????????????????????????????????????
    private static void EstiloEncabezado(IXLWorksheet ws, int fila, int columnas)
    {
        var rango = ws.Range(fila, 1, fila, columnas);
        rango.Style.Font.Bold = true;
        rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#0d6efd");
        rango.Style.Font.FontColor = XLColor.White;
        rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rango.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        rango.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
    }

    private static void AgregarHojaResumen(XLWorkbook workbook, List<Accion> acciones, DateTime desde, DateTime hasta)
    {
        var ws = workbook.Worksheets.Add("Resumen");

        ws.Cell(1, 1).Value = "Resumen del Reporte";
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 14;

        ws.Cell(3, 1).Value = "Periodo:";
        ws.Cell(3, 2).Value = $"{desde:dd/MM/yyyy} - {hasta:dd/MM/yyyy}";

        ws.Cell(4, 1).Value = "Total acciones:";
        ws.Cell(4, 2).Value = acciones.Count;

        ws.Cell(6, 1).Value = "Estado";
        ws.Cell(6, 2).Value = "Cantidad";
        EstiloEncabezado(ws, 6, 2);

        var porEstado = acciones.GroupBy(a => a.Estado?.Descripcion ?? "-")
            .OrderBy(g => g.Key);

        int fila = 7;
        foreach (var grupo in porEstado)
        {
            ws.Cell(fila, 1).Value = grupo.Key;
            ws.Cell(fila, 2).Value = grupo.Count();
            fila++;
        }

        fila += 2;
        ws.Cell(fila, 1).Value = "Tipo Accion";
        ws.Cell(fila, 2).Value = "Cantidad";
        EstiloEncabezado(ws, fila, 2);
        fila++;

        var porTipo = acciones.GroupBy(a => a.TipoAccionId);
        foreach (var grupo in porTipo)
        {
            ws.Cell(fila, 1).Value = grupo.Key switch { 1 => "Preventiva", 2 => "Correctiva", 3 => "Mejora", _ => "-" };
            ws.Cell(fila, 2).Value = grupo.Count();
            fila++;
        }

        ws.Columns().AdjustToContents();
    }

    private static byte[] WorkbookToBytes(XLWorkbook workbook)
    {
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
