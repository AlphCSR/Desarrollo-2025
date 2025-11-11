using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using NotificationsMS.Domain.Entities;
using NotificationsMS.Commons.Events;
using NotificationsMS.Core.Service;
using Microsoft.Extensions.Logging;

namespace NotificationsMS.Infrastructure.Service
{
    /// <summary>
    /// Implementación del publicador de eventos para notificaciones.
    /// Utiliza MassTransit para publicar eventos de creación y actualización de notificaciones.
    /// </summary>
    public class EventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<EventPublisher> _logger;

        /// <summary>
        /// Constructor de <see cref="EventPublisher"/>.
        /// </summary>
        /// <param name="publishEndpoint">Punto de publicación de MassTransit.</param>
        /// <param name="logger">Instancia de logger para registrar eventos.</param>
        public EventPublisher(IPublishEndpoint publishEndpoint, ILogger<EventPublisher> logger)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("EventPublisher instanciado.");
        }

        /// <summary>
        /// Publica un evento cuando una notificación ha sido creada.
        /// </summary>
        /// <param name="notification">La notificación que fue creada.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <exception cref="ApplicationException">Se lanza si ocurre un error al publicar el evento.</exception>
        public async Task PublishNotificationCreatedAsync(Notification notification)
        {
            _logger.LogInformation("Intentando publicar evento de notificación creada para IdNotification: {IdNotification}", notification.IdNotification);
            var @event = new NotificationCreatedEvent
            {
                IdNotification = notification.IdNotification,
                IdUser = notification.IdUser,
                Message = notification.Message,
                State = notification.State,    
                CreatedAt = notification.CreatedAt,
            };

            try
            {
                await _publishEndpoint.Publish(@event);
                _logger.LogInformation("Evento NotificationCreatedEvent publicado exitosamente para IdNotification: {IdNotification}", notification.IdNotification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al publicar NotificationCreatedEvent para IdNotification: {IdNotification}", notification.IdNotification);
                throw new ApplicationException($"Error al publicar el evento de notificación creada para ID {notification.IdNotification}.", ex);
            }
        }

        /// <summary>
        /// Publica un evento cuando una notificación ha sido actualizada.
        /// </summary>
        /// <param name="notification">La notificación que fue actualizada.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <exception cref="ApplicationException">Se lanza si ocurre un error al publicar el evento.</exception>
        public async Task PublishNotificationUpdatedAsync(Notification notification)
        {
            _logger.LogInformation("Intentando publicar evento de notificación actualizada para IdNotification: {IdNotification}", notification.IdNotification);
            var @event = new NotificationUpdateEvent
            {
                IdNotification = notification.IdNotification,
                IdUser = notification.IdUser,
                Message = notification.Message,
                State = notification.State,    
                CreatedAt = notification.CreatedAt,
            };

            try
            {
                await _publishEndpoint.Publish(@event);
                _logger.LogInformation("Evento NotificationUpdateEvent publicado exitosamente para IdNotification: {IdNotification}", notification.IdNotification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al publicar NotificationUpdateEvent para IdNotification: {IdNotification}", notification.IdNotification);
                throw new ApplicationException($"Error al publicar el evento de notificación actualizada para ID {notification.IdNotification}.", ex);
            }
        }
    }
}

