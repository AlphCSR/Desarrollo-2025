
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using NotificationsMS.Core.Messaging.Sender;
using Microsoft.Extensions.Logging;

namespace NotificationsMS.Infrastructure.Messaging.Sender
{
    /// <summary>
    /// Implementación del servicio de envío de correos electrónicos.
    /// Se encarga de enviar notificaciones por correo electrónico utilizando un cliente SMTP.
    /// </summary>
    public class Email : IEmail
    {
        private readonly ILogger<Email> _logger;

        /// <summary>
        /// Constructor de <see cref="Email"/>.
        /// </summary>
        /// <param name="logger">Instancia de logger para registrar eventos.</param>
        public Email(ILogger<Email> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("Email service instanciado.");
        }

        /// <summary>
        /// Envía un correo electrónico de forma asíncrona.
        /// </summary>
        /// <param name="email">La dirección de correo electrónico del destinatario.</param>
        /// <param name="body">El cuerpo del mensaje del correo electrónico.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <exception cref="ApplicationException">Se lanza si ocurre un error al enviar el correo electrónico.</exception>
        public async Task SendEmailAsync(string email, string body)
        {
            _logger.LogInformation("Intentando enviar correo electrónico a: {EmailAddress}", email);

            // TODO: Considerar mover las credenciales y la configuración SMTP a un archivo de configuración (ej. appsettings.json)
            // y usar IOptions<T> para inyectarlas, en lugar de hardcodearlas aquí.
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("ctroz.ch@gmail.com", "bexu buhx qvhr hwuh"),
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("ctroz.ch@gmail.com"),
                Subject = "Notification",
                Body = body,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Correo electrónico enviado exitosamente a: {EmailAddress}", email);
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "Error SMTP al enviar correo electrónico a {EmailAddress}: {Message}", email, ex.Message);
                throw new ApplicationException($"Error SMTP al enviar correo electrónico a {email}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al enviar correo electrónico a {EmailAddress}: {Message}", email, ex.Message);
                throw new ApplicationException($"Error inesperado al enviar correo electrónico a {email}.", ex);
            }
        }
    }
}