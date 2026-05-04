using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace ActividadApp.Services.Email;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    private Task<bool> EnviarCorreo(string destinatarioEmail, string asunto, string cuerpoHtml)
    {
        try
        {
            using var client = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort)
            {
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password),
                EnableSsl = _settings.UseSsl,
                Timeout = 10000
            };

            using var message = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = asunto,
                Body = cuerpoHtml,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(destinatarioEmail));

            return client.SendMailAsync(message).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    _logger.LogError(task.Exception, "Error al enviar correo a {Email}: {Asunto}", destinatarioEmail, asunto);
                    return false;
                }

                _logger.LogInformation("Correo enviado a {Email}: {Asunto}", destinatarioEmail, asunto);
                return true;
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al preparar correo a {Email}: {Asunto}", destinatarioEmail, asunto);
            return Task.FromResult(false);
        }


    }

    public async Task<bool> EnviarAccionCreada(string destinatarioEmail, string destinatarioNombre,
        string consecutivo, string proceso, string descripcion, string tipoAccion, DateTime fecha)
    {
        var (asunto, cuerpo) = EmailTemplates.AccionCreada(
            destinatarioNombre, consecutivo, proceso, descripcion, tipoAccion, fecha);
        return await EnviarCorreo(destinatarioEmail, asunto, cuerpo);
    }

    public async Task<bool> EnviarSolucionRegistrada(string destinatarioEmail, string destinatarioNombre,
        string consecutivo, string proceso, string descripcionActividad, string agencia, DateTime fechaCompromiso)
    {
        var (asunto, cuerpo) = EmailTemplates.SolucionRegistrada(
            destinatarioNombre, consecutivo, proceso, descripcionActividad, agencia, fechaCompromiso);
        return await EnviarCorreo(destinatarioEmail, asunto, cuerpo);
    }

    public async Task<bool> EnviarConfirmacionPendiente(string destinatarioEmail, string destinatarioNombre,
        string consecutivo, string proceso, string descripcion)
    {
        var (asunto, cuerpo) = EmailTemplates.ConfirmacionPendiente(
            destinatarioNombre, consecutivo, proceso, descripcion);
        return await EnviarCorreo(destinatarioEmail, asunto, cuerpo);
    }

    public async Task<bool> EnviarEficaciaPendiente(string destinatarioEmail, string destinatarioNombre,
        string consecutivo, string proceso, string descripcion)
    {
        var (asunto, cuerpo) = EmailTemplates.EficaciaPendiente(
            destinatarioNombre, consecutivo, proceso, descripcion);
        return await EnviarCorreo(destinatarioEmail, asunto, cuerpo);
    }

    public async Task<bool> EnviarAccionCerrada(string destinatarioEmail, string destinatarioNombre,
        string consecutivo, string proceso, string descripcion, bool eficaz)
    {
        var (asunto, cuerpo) = EmailTemplates.AccionCerrada(
            destinatarioNombre, consecutivo, proceso, descripcion, eficaz);
        return await EnviarCorreo(destinatarioEmail, asunto, cuerpo);
    }

    public async Task<bool> EnviarActividadEjecutada(string destinatarioEmail, string destinatarioNombre,
        string consecutivo, string proceso, string descripcionActividad, DateTime fechaEjecucion)
    {
        var (asunto, cuerpo) = EmailTemplates.ActividadEjecutada(
            destinatarioNombre, consecutivo, proceso, descripcionActividad, fechaEjecucion);
        return await EnviarCorreo(destinatarioEmail, asunto, cuerpo);
    }

    public async Task<bool> EnviarRecordatorio(string destinatarioEmail, string destinatarioNombre,
        string consecutivo, string proceso, string descripcion, string estado, int diasPendiente)
    {
        var (asunto, cuerpo) = EmailTemplates.Recordatorio(
            destinatarioNombre, consecutivo, proceso, descripcion, estado, diasPendiente);
        return await EnviarCorreo(destinatarioEmail, asunto, cuerpo);
    }
}
