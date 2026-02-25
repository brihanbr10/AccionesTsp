namespace ActividadApp.Services.Email;

public static class EmailTemplates
{
    private static string BaseLayout(string titulo, string contenido) => $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='margin:0; padding:0; background-color:#f4f5f7; font-family:Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif;'>
    <table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f4f5f7; padding:30px 0;'>
        <tr>
            <td align='center'>
                <table width='600' cellpadding='0' cellspacing='0' style='background-color:#ffffff; border-radius:8px; overflow:hidden; box-shadow:0 2px 8px rgba(0,0,0,0.08);'>
                    <!-- Header -->
                    <tr>
                        <td style='background: linear-gradient(135deg, #0d6efd, #0a58ca); padding:24px 32px;'>
                            <h1 style='margin:0; color:#ffffff; font-size:20px; font-weight:600;'>
                                Sistema de Acciones
                            </h1>
                        </td>
                    </tr>
                    <!-- Titulo -->
                    <tr>
                        <td style='padding:28px 32px 8px 32px;'>
                            <h2 style='margin:0; color:#212529; font-size:18px; font-weight:600;'>{titulo}</h2>
                        </td>
                    </tr>
                    <!-- Contenido -->
                    <tr>
                        <td style='padding:16px 32px 28px 32px; color:#495057; font-size:14px; line-height:1.6;'>
                            {contenido}
                        </td>
                    </tr>
                    <!-- Footer -->
                    <tr>
                        <td style='background-color:#f8f9fa; padding:16px 32px; border-top:1px solid #e9ecef;'>
                            <p style='margin:0; color:#6c757d; font-size:12px; text-align:center;'>
                                Este es un mensaje automatico del Sistema de Acciones. No responda a este correo.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";

    private static string InfoRow(string label, string valor) =>
        $@"<tr>
            <td style='padding:6px 12px; color:#6c757d; font-size:13px; white-space:nowrap; vertical-align:top;'><strong>{label}</strong></td>
            <td style='padding:6px 12px; color:#212529; font-size:13px;'>{valor}</td>
        </tr>";

    private static string InfoTable(params (string label, string valor)[] filas)
    {
        var rows = string.Join("", filas.Select(f => InfoRow(f.label, f.valor)));
        return $@"<table cellpadding='0' cellspacing='0' style='width:100%; background-color:#f8f9fa; border-radius:6px; border:1px solid #e9ecef; margin:12px 0;'>
            {rows}
        </table>";
    }

    private static string Badge(string texto, string bgColor) =>
        $@"<span style='display:inline-block; padding:4px 12px; background-color:{bgColor}; color:#ffffff; border-radius:12px; font-size:12px; font-weight:600;'>{texto}</span>";

    public static (string asunto, string cuerpo) AccionCreada(
        string nombre, string consecutivo, string proceso, string descripcion, string tipoAccion, DateTime fecha)
    {
        var asunto = $"Nueva Accion {consecutivo} - Requiere su atencion";
        var contenido = $@"
            <p>Estimado(a) <strong>{nombre}</strong>,</p>
            <p>Se ha creado una nueva accion que requiere su atencion como responsable del proceso:</p>
            {InfoTable(
                ("Consecutivo", consecutivo),
                ("Tipo", tipoAccion),
                ("Proceso", proceso),
                ("Fecha", fecha.ToString("dd/MM/yyyy")),
                ("Descripcion", descripcion)
            )}
            <p>Por favor ingrese al sistema para dar tratamiento a esta accion.</p>
            <p style='margin-top:16px;'>{Badge("Abierta sin Plan", "#6c757d")}</p>";
        return (asunto, BaseLayout("Nueva Accion Asignada", contenido));
    }

    public static (string asunto, string cuerpo) SolucionRegistrada(
        string nombre, string consecutivo, string proceso, string descripcionActividad, string agencia, DateTime fechaCompromiso)
    {
        var asunto = $"Actividad asignada - Accion {consecutivo}";
        var contenido = $@"
            <p>Estimado(a) <strong>{nombre}</strong>,</p>
            <p>Se le ha asignado una actividad dentro del plan de accion:</p>
            {InfoTable(
                ("Accion", consecutivo),
                ("Proceso", proceso),
                ("Actividad", descripcionActividad),
                ("Agencia", agencia),
                ("Fecha compromiso", fechaCompromiso.ToString("dd/MM/yyyy"))
            )}
            <p>Por favor ejecute la actividad asignada antes de la fecha de compromiso.</p>
            <p style='margin-top:16px;'>{Badge("Abierta con Solucion", "#0d6efd")}</p>";
        return (asunto, BaseLayout("Actividad del Plan de Accion", contenido));
    }

    public static (string asunto, string cuerpo) ConfirmacionPendiente(
        string nombre, string consecutivo, string proceso, string descripcion)
    {
        var asunto = $"Confirmacion pendiente - Accion {consecutivo}";
        var contenido = $@"
            <p>Estimado(a) <strong>{nombre}</strong>,</p>
            <p>Todas las actividades del plan de accion han sido ejecutadas. Se requiere su seguimiento para confirmar el plan:</p>
            {InfoTable(
                ("Accion", consecutivo),
                ("Proceso", proceso),
                ("Descripcion", descripcion)
            )}
            <p>Por favor ingrese al sistema para realizar la confirmacion del plan de accion.</p>
            <p style='margin-top:16px;'>{Badge("Abierta por Confirmacion", "#ffc107")}</p>";
        return (asunto, BaseLayout("Confirmacion del Plan de Accion", contenido));
    }

    public static (string asunto, string cuerpo) EficaciaPendiente(
        string nombre, string consecutivo, string proceso, string descripcion)
    {
        var asunto = $"Verificacion de eficacia pendiente - Accion {consecutivo}";
        var contenido = $@"
            <p>Estimado(a) <strong>{nombre}</strong>,</p>
            <p>El plan de accion ha sido confirmado. Se requiere la verificacion de eficacia:</p>
            {InfoTable(
                ("Accion", consecutivo),
                ("Proceso", proceso),
                ("Descripcion", descripcion)
            )}
            <p>Por favor ingrese al sistema para verificar la eficacia de la accion.</p>
            <p style='margin-top:16px;'>{Badge("Pendiente por Eficacia", "#0dcaf0")}</p>";
        return (asunto, BaseLayout("Verificacion de Eficacia", contenido));
    }

    public static (string asunto, string cuerpo) AccionCerrada(
        string nombre, string consecutivo, string proceso, string descripcion, bool eficaz)
    {
        var resultado = eficaz ? "EFICAZ" : "NO EFICAZ";
        var colorResultado = eficaz ? "#198754" : "#dc3545";
        var asunto = $"Accion {consecutivo} cerrada - {resultado}";
        var contenido = $@"
            <p>Estimado(a) <strong>{nombre}</strong>,</p>
            <p>La accion ha sido cerrada con el siguiente resultado:</p>
            {InfoTable(
                ("Accion", consecutivo),
                ("Proceso", proceso),
                ("Descripcion", descripcion),
                ("Resultado", $"<strong style='color:{colorResultado};'>{resultado}</strong>")
            )}
            <p style='margin-top:16px;'>{Badge("Cerrada", "#198754")}</p>";
        return (asunto, BaseLayout("Accion Cerrada", contenido));
    }

    public static (string asunto, string cuerpo) Recordatorio(
        string nombre, string consecutivo, string proceso, string descripcion, string estado, int diasPendiente)
    {
        var asunto = $"RECORDATORIO - Accion {consecutivo} pendiente hace {diasPendiente} dias";
        var contenido = $@"
            <p>Estimado(a) <strong>{nombre}</strong>,</p>
            <p style='color:#dc3545;'><strong>? Esta accion lleva {diasPendiente} dias sin avance.</strong></p>
            <p>Le recordamos que la siguiente accion requiere su atencion:</p>
            {InfoTable(
                ("Accion", consecutivo),
                ("Proceso", proceso),
                ("Descripcion", descripcion),
                ("Estado actual", estado),
                ("Dias pendiente", diasPendiente.ToString())
            )}
            <p>Por favor ingrese al sistema lo antes posible para dar seguimiento.</p>";
        return (asunto, BaseLayout("Recordatorio de Accion Pendiente", contenido));
    }
}
