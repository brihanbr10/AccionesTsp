namespace ActividadApp.Services.Email;

public interface IEmailService
{
    Task<bool> EnviarAccionCreada(string destinatarioEmail, string destinatarioNombre,
        string consecutivo, string proceso, string descripcion, string tipoAccion, DateTime fecha);

    Task<bool> EnviarSolucionRegistrada(string destinatarioEmail, string destinatarioNombre,
        string consecutivo, string proceso, string descripcionActividad, string agencia, DateTime fechaCompromiso);

    Task<bool> EnviarConfirmacionPendiente(string destinatarioEmail, string destinatarioNombre,
        string consecutivo, string proceso, string descripcion);

    Task<bool> EnviarEficaciaPendiente(string destinatarioEmail, string destinatarioNombre,
        string consecutivo, string proceso, string descripcion);

    Task<bool> EnviarAccionCerrada(string destinatarioEmail, string destinatarioNombre,
        string consecutivo, string proceso, string descripcion, bool eficaz);

    Task<bool> EnviarRecordatorio(string destinatarioEmail, string destinatarioNombre,
        string consecutivo, string proceso, string descripcion, string estado, int diasPendiente);
}
