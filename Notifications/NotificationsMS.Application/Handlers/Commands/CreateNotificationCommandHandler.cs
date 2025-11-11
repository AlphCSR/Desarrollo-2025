using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using NotificationsMS.Application.Commands;
using NotificationsMS.Core.Repositories;
using NotificationsMS.Core.Service;
using NotificationsMS.Core.Messaging.Sender;
using NotificationsMS.Domain.Entities;
using NotificationsMS.Infrastructure.Service;
using NotificationsMS.Infrastructure.Messaging.Sender;
using Microsoft.Extensions.Logging;

namespace NotificationsMS.Application.Handlers.Commands
{
    /// <summary>
    /// Manejador para el comando de creación de notificaciones.
    /// Se encarga de persistir la notificación, publicar un evento y enviar un correo electrónico.
    /// </summary>
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, string>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEmail _emailService;
        private readonly ILogger<CreateNotificationCommandHandler> _logger;

        /// <summary>
        /// Constructor de CreateNotificationCommandHandler.
        /// </summary>
        /// <param name="notificationRepository">Repositorio para la gestión de notificaciones.</param>
        /// <param name="eventPublisher">Publicador de eventos para notificaciones.</param>
        /// <param name="emailService">Servicio para el envío de correos electrónicos.</param>
        /// <param name="logger">Instancia de logger para registrar eventos.</param>
        public CreateNotificationCommandHandler(
            INotificationRepository notificationRepository,
            IEventPublisher eventPublisher,
            IEmail emailService,
            ILogger<CreateNotificationCommandHandler> logger)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("CreateNotificationCommandHandler instanciado.");
        }

        /// <summary>
        /// Maneja el comando de creación de notificación.
        /// </summary>
        /// <param name="request">El comando que contiene los datos de la nueva notificación.</param>
        /// <param name="cancellationToken">Token para cancelar la operación.</param>
        /// <returns>Un mensaje de éxito si la notificación se crea correctamente.</returns>
        public async Task<string> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando manejo del comando CreateNotificationCommand para IdNotification: {IdNotification}", request.CreateNotificationDto.IdNotification);

            var dto = request.CreateNotificationDto;

            var notification = new Notification(
                dto.IdNotification,
                dto.IdUser,
                dto.Message,
                dto.State,
                dto.CreatedAt
            )
            {
                State = dto.State
            };

            try
            {
                _logger.LogInformation("Intentando agregar notificación a la base de datos.");
                await _notificationRepository.AddAsync(notification);
                _logger.LogInformation("Notificación agregada a la base de datos exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar notificación a la base de datos para IdNotification: {IdNotification}", dto.IdNotification);
                throw new ApplicationException("Error al persistir la notificación.", ex);
            }

            try
            {
                _logger.LogInformation("Intentando publicar evento de notificación creada.");
                await _eventPublisher.PublishNotificationCreatedAsync(notification);
                _logger.LogInformation("Evento de notificación creada publicado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al publicar evento de notificación creada para IdNotification: {IdNotification}", dto.IdNotification);
                // Considerar si este error debe ser fatal o solo logueado (ej. retry mechanism)
            }

            try
            {
                _logger.LogInformation("Intentando enviar correo electrónico a {Email}", dto.Email);
                await _emailService.SendEmailAsync(dto.Email, dto.Message);
                _logger.LogInformation("Correo electrónico enviado exitosamente a {Email}", dto.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar correo electrónico a {Email} para IdNotification: {IdNotification}", dto.Email, dto.IdNotification);
                // Considerar si este error debe ser fatal o solo logueado
            }
            
            _logger.LogInformation("Comando CreateNotificationCommand manejado exitosamente para IdNotification: {IdNotification}", request.CreateNotificationDto.IdNotification);
            return "Notification successfully created.";
        }
    }
}
