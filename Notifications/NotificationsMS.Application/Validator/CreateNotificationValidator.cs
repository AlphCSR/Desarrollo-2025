using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsMS.Commons.Dtos.Request;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace NotificationsMS.Application.Validator
{
    /// <summary>
    /// Validador para el DTO de creación de notificaciones (<see cref="CreateNotificationDto"/>).
    /// Define las reglas de validación para los campos requeridos al crear una nueva notificación.
    /// </summary>
    public class CreateNotificationValidator : ValidatorBase<CreateNotificationDto>
    {
        private readonly ILogger<CreateNotificationValidator> _logger;

        /// <summary>
        /// Constructor de <see cref="CreateNotificationValidator"/>.
        /// </summary>
        /// <param name="logger">Instancia de logger para registrar eventos.</param>
        public CreateNotificationValidator(ILogger<CreateNotificationValidator> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("CreateNotificationValidator instanciado. Definiendo reglas de validación.");

            RuleFor(s => s.IdUser)
                .NotNull().WithMessage("IdUser no puede ser nulo").WithErrorCode("040")
                .NotEmpty().WithMessage("IdUser no puede estar vacio").WithErrorCode("041");

            RuleFor(s => s.Email)
                .NotNull().WithMessage("El correo electrónico es requerido").WithErrorCode("042")
                .NotEmpty().WithMessage("El correo electrónico no puede estar vacío").WithErrorCode("043")
                .Must(BeAValidEmail).WithMessage("El formato del correo electrónico no es válido").WithErrorCode("044");

            RuleFor(s => s.Message)
                .NotNull().WithMessage("Message no puede ser nulo").WithErrorCode("045")
                .NotEmpty().WithMessage("Message no puede estar vacio").WithErrorCode("046");

            RuleFor(s => s.CreatedAt)
                .NotNull().WithMessage("CreatedAt no puede ser nulo").WithErrorCode("055")
                .NotEmpty().WithMessage("CreatedAt no puede estar vacio").WithErrorCode("056");

            _logger.LogInformation("Reglas de validación para CreateNotificationDto definidas.");
        }

        /// <summary>
        /// Valida si una cadena de texto es un formato de correo electrónico válido.
        /// </summary>
        /// <param name="email">La cadena de texto a validar como correo electrónico.</param>
        /// <returns>True si el formato es válido, de lo contrario False.</returns>
        private bool BeAValidEmail(string email)
        {
            _logger.LogDebug("Validando formato de correo electrónico: {Email}", email);
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Correo electrónico nulo o vacío.");
                return false;
            }

            // Expresión regular para una validación de correo electrónico más robusta
            // Fuente: https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            var isValid = regex.IsMatch(email);
            
            if (!isValid)
            {
                _logger.LogWarning("Formato de correo electrónico inválido: {Email}", email);
            }
            else
            {
                _logger.LogDebug("Formato de correo electrónico válido: {Email}", email);
            }

            return isValid;
        }
    }
}
