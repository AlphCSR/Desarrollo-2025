using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using NotificationsMS.Commons.Dtos.Response;
using NotificationsMS.Core.Repositories;
using NotificationsMS.Application.Querys;
using NotificationsMS.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;

namespace NotificationsMS.Application.Handlers.Querys
{
    /// <summary>
    /// Manejador para la consulta de una notificación específica por su ID.
    /// Se encarga de recuperar la notificación del repositorio y mapearla a un DTO de respuesta.
    /// </summary>
    public class GetNotificationQueryHandler : IRequestHandler<GetNotificationQuery, GetNotificationDto>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<GetNotificationQueryHandler> _logger;

        /// <summary>
        /// Constructor de GetNotificationQueryHandler.
        /// </summary>
        /// <param name="notificationRepository">Repositorio para la gestión de notificaciones.</param>
        /// <param name="logger">Instancia de logger para registrar eventos.</param>
        public GetNotificationQueryHandler(
            INotificationRepository notificationRepository,
            ILogger<GetNotificationQueryHandler> logger)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("GetNotificationQueryHandler instanciado.");
        }

        /// <summary>
        /// Maneja la consulta para obtener una notificación por su ID.
        /// </summary>
        /// <param name="request">La consulta que contiene el ID de la notificación.</param>
        /// <param name="cancellationToken">Token para cancelar la operación.</param>
        /// <returns>Un DTO de la notificación encontrada.</returns>
        /// <exception cref="NotificationNotFoundException">Se lanza si la notificación con el ID especificado no se encuentra.</exception>
        public async Task<GetNotificationDto> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando manejo de la consulta GetNotificationQuery para IdNotification: {IdNotification}", request.IdNotification);

            Domain.Entities.Notification notification;
            try
            {
                _logger.LogInformation("Buscando notificación con ID: {IdNotification} en el repositorio.", request.IdNotification);
                notification = await _notificationRepository.GetByIdAsync(request.IdNotification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar notificación con ID {IdNotification} en el repositorio.", request.IdNotification);
                throw new ApplicationException($"Error al recuperar la notificación con ID {request.IdNotification}.", ex);
            }

            if (notification == null)
            {
                _logger.LogWarning("Notificación con ID {IdNotification} no encontrada.", request.IdNotification);
                throw new NotificationNotFoundException($"Notificación con ID {request.IdNotification} no encontrada.");
            }

            _logger.LogInformation("Notificación con ID {IdNotification} encontrada. Mapeando a DTO de respuesta.", request.IdNotification);
            var result = new GetNotificationDto
            {
                IdNotification = notification.IdNotification,
                IdUser = notification.IdUser,
                Message = notification.Message,
                State = notification.State,
                CreatedAt = notification.CreatedAt,
            };

            _logger.LogInformation("Consulta GetNotificationQuery manejada exitosamente para IdNotification: {IdNotification}", request.IdNotification);
            return result;
        }
    }
}
