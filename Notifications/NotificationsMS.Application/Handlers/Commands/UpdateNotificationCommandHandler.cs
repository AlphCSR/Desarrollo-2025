using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using NotificationsMS.Application.Commands;
using NotificationsMS.Core.Repositories;
using NotificationsMS.Core.Service;
using NotificationsMS.Domain.Entities;
using NotificationsMS.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;

namespace NotificationsMS.Application.Handlers.Commands
{
    /// <summary>
    /// Manejador para el comando de actualización de notificaciones.
    /// Se encarga de buscar la notificación, actualizar sus datos y publicar un evento de actualización.
    /// </summary>
    public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, string>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<UpdateNotificationCommandHandler> _logger;

        /// <summary>
        /// Constructor de UpdateNotificationCommandHandler.
        /// </summary>
        /// <param name="notificationRepository">Repositorio para la gestión de notificaciones.</param>
        /// <param name="eventPublisher">Publicador de eventos para notificaciones.</param>
        /// <param name="logger">Instancia de logger para registrar eventos.</param>
        public UpdateNotificationCommandHandler(
            INotificationRepository notificationRepository,
            IEventPublisher eventPublisher,
            ILogger<UpdateNotificationCommandHandler> logger)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("UpdateNotificationCommandHandler instanciado.");
        }

        /// <summary>
        /// Maneja el comando de actualización de notificación.
        /// </summary>
        /// <param name="request">El comando que contiene los datos de la notificación a actualizar.</param>
        /// <param name="cancellationToken">Token para cancelar la operación.</param>
        /// <returns>Un mensaje de éxito si la notificación se actualiza correctamente.</returns>
        /// <exception cref="NotificationNotFoundException">Se lanza si la notificación no se encuentra.</exception>
        public async Task<string> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando manejo del comando UpdateNotificationCommand para IdNotification: {IdNotification}", request.UpdateNotificationDto.IdNotification);

            Notification notification;
            try
            {
                _logger.LogInformation("Buscando notificación con ID: {IdNotification}", request.UpdateNotificationDto.IdNotification);
                notification = await _notificationRepository.GetByIdAsync(request.UpdateNotificationDto.IdNotification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar notificación con ID {IdNotification}", request.UpdateNotificationDto.IdNotification);
                throw new ApplicationException($"Error al buscar la notificación con ID {request.UpdateNotificationDto.IdNotification}.", ex);
            }

            if (notification == null)
            {
                _logger.LogWarning("Notificación con ID {IdNotification} no encontrada para actualización.", request.UpdateNotificationDto.IdNotification);
                throw new NotificationNotFoundException($"Notificación con ID {request.UpdateNotificationDto.IdNotification} no encontrada.");
            }

            if (!string.IsNullOrWhiteSpace(request.UpdateNotificationDto.Message))
            {
                notification.Message = request.UpdateNotificationDto.Message;
            }

            notification.State = request.UpdateNotificationDto.State;

            try
            {
                _logger.LogInformation("Intentando actualizar notificación en la base de datos para IdNotification: {IdNotification}", notification.IdNotification);
                await _notificationRepository.UpdateAsync(notification);
                _logger.LogInformation("Notificación actualizada en la base de datos exitosamente para IdNotification: {IdNotification}", notification.IdNotification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar notificación en la base de datos para IdNotification: {IdNotification}", notification.IdNotification);
                throw new ApplicationException($"Error al actualizar la notificación con ID {notification.IdNotification}.", ex);
            }

            try
            {
                _logger.LogInformation("Intentando publicar evento de notificación actualizada para IdNotification: {IdNotification}", notification.IdNotification);
                await _eventPublisher.PublishNotificationUpdatedAsync(notification);
                _logger.LogInformation("Evento de notificación actualizada publicado exitosamente para IdNotification: {IdNotification}", notification.IdNotification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al publicar evento de notificación actualizada para IdNotification: {IdNotification}", notification.IdNotification);
                // Considerar si este error debe ser fatal o solo logueado (ej. retry mechanism)
            }

            _logger.LogInformation("Comando UpdateNotificationCommand manejado exitosamente para IdNotification: {IdNotification}", request.UpdateNotificationDto.IdNotification);
            return "Notification updated successfully.";
        }
    }
}