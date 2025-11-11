using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NotificationsMS.Application.Querys;
using NotificationsMS.Commons.Dtos.Response;
using NotificationsMS.Core.Repositories;
using NotificationsMS.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;

namespace NotificationsMS.Application.Handlers.Querys
{
    /// <summary>
    /// Manejador para la consulta de todas las notificaciones.
    /// Se encarga de recuperar todas las notificaciones del repositorio y mapearlas a DTOs de respuesta.
    /// </summary>
    public class GetAllNotificationQueryHandler : IRequestHandler<GetAllNotificationQuery, List<GetAllNotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<GetAllNotificationQueryHandler> _logger;

        /// <summary>
        /// Constructor de GetAllNotificationQueryHandler.
        /// </summary>
        /// <param name="notificationRepository">Repositorio para la gestión de notificaciones.</param>
        /// <param name="logger">Instancia de logger para registrar eventos.</param>
        public GetAllNotificationQueryHandler(
            INotificationRepository notificationRepository,
            ILogger<GetAllNotificationQueryHandler> logger)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("GetAllNotificationQueryHandler instanciado.");
        }

        /// <summary>
        /// Maneja la consulta para obtener todas las notificaciones.
        /// </summary>
        /// <param name="request">La consulta para obtener todas las notificaciones.</param>
        /// <param name="cancellationToken">Token para cancelar la operación.</param>
        /// <returns>Una lista de DTOs de todas las notificaciones.</returns>
        /// <exception cref="NotificationNotFoundException">Se lanza si no se encuentran notificaciones.</exception>
        public async Task<List<GetAllNotificationDto>> Handle(GetAllNotificationQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando manejo de la consulta GetAllNotificationQuery.");

            List<Domain.Entities.Notification> notifications;
            try
            {
                _logger.LogInformation("Intentando obtener todas las notificaciones del repositorio.");
                notifications = await _notificationRepository.GetAllAsync();
                _logger.LogInformation("Notificaciones obtenidas del repositorio. Cantidad: {Count}", notifications?.Count ?? 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las notificaciones del repositorio.");
                throw new ApplicationException("Error al recuperar las notificaciones.", ex);
            }

            if (notifications == null || !notifications.Any())
            {
                _logger.LogWarning("No se encontraron notificaciones.");
                throw new NotificationNotFoundException("No se encontraron notificaciones.");
            }

            _logger.LogInformation("Mapeando notificaciones a DTOs de respuesta.");
            var result = notifications.Select(notification => new GetAllNotificationDto
            {
                IdNotification = notification.IdNotification,
                IdUser = notification.IdUser,
                Message = notification.Message,
                State = notification.State,
                CreatedAt = notification.CreatedAt,
            }).ToList();

            _logger.LogInformation("Consulta GetAllNotificationQuery manejada exitosamente. Retornando {Count} notificaciones.", result.Count);
            return result;
        }
    }
}
