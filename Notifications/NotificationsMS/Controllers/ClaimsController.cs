using Microsoft.AspNetCore.Mvc;
using MediatR;
using NotificationsMS.Commons.Dtos.Request;
using NotificationsMS.Application.Commands;
using NotificationsMS.Infrastructure.Exceptions;
using NotificationsMS.Application.Querys;
using Microsoft.AspNetCore.Authorization;


namespace NotificationsMS.Controllers
{
    [ApiController]
    [Route("notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly ILogger<NotificationsController> _logger;
        private readonly IMediator _mediator;

        public NotificationsController(ILogger<NotificationsController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger.LogInformation("NotificationsController instantiated");
        }

        /// <summary>
        /// Crea una nueva notificación en el sistema.
        /// </summary>
        /// <param name="createNotificationDto">Datos para la creación de la notificación.</param>
        /// <returns>Un mensaje de éxito si la notificación se crea correctamente.</returns>
        /// <response code="200">Retorna un mensaje de éxito.</response>
        /// <response code="400">Si los datos de entrada no son válidos.</response>
        /// <response code="500">Si ocurre un error interno del servidor.</response>
        [HttpPost("create-notification")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto createNotificationDto)
        {
            _logger.LogInformation("Inicio de la creación de notificación. Datos recibidos: {@NotificationDto}", createNotificationDto);
            try
            {
                var command = new CreateNotificationCommand(createNotificationDto);
                var message = await _mediator.Send(command);
                _logger.LogInformation("Notificación creada exitosamente. Mensaje: {Message}", message);
                return Ok(message);
            }
            catch (ValidatorException ex)
            {
                _logger.LogWarning(ex, "Error de validación al crear notificación: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear notificación: {Message}", ex.Message);
                return StatusCode(500, "Ocurrió un error interno al crear la notificación.");
            }
        }

        /// <summary>
        /// Actualiza una notificación existente en el sistema.
        /// </summary>
        /// <param name="updateNotificationDto">Datos para la actualización de la notificación.</param>
        /// <returns>Un mensaje de éxito si la notificación se actualiza correctamente.</returns>
        /// <response code="200">Retorna un mensaje de éxito.</response>
        /// <response code="400">Si los datos de entrada no son válidos.</response>
        /// <response code="404">Si la notificación no se encuentra.</response>
        /// <response code="500">Si ocurre un error interno del servidor.</response>
        [HttpPut("update-notification")]
        public async Task<IActionResult> UpdateNotification([FromBody] UpdateNotificationDto updateNotificationDto)
        {
            _logger.LogInformation("Inicio de la actualización de notificación. Datos recibidos: {@NotificationDto}", updateNotificationDto);
            try
            {
                var command = new UpdateNotificationCommand(updateNotificationDto);
                var message = await _mediator.Send(command);
                _logger.LogInformation("Notificación actualizada exitosamente. Mensaje: {Message}", message);
                return Ok(message);
            }
            catch (NotificationNotFoundException ex)
            {
                _logger.LogWarning(ex, "Notificación no encontrada para actualización: {Message}", ex.Message);
                return NotFound(ex.Message);
            }
            catch (ValidatorException ex)
            {
                _logger.LogWarning(ex, "Error de validación al actualizar notificación: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar notificación: {Message}", ex.Message);
                return StatusCode(500, "Ocurrió un error interno al actualizar la notificación.");
            }
        }

        /// <summary>
        /// Obtiene todas las notificaciones existentes.
        /// </summary>
        /// <returns>Una lista de todas las notificaciones.</returns>
        /// <response code="200">Retorna la lista de notificaciones.</response>
        /// <response code="500">Si ocurre un error interno del servidor.</response>
        [HttpGet("get-all-notifications")]
        public async Task<IActionResult> GetAllNotifications()
        {
            _logger.LogInformation("Inicio de la obtención de todas las notificaciones.");
            try
            {
                var query = new GetAllNotificationQuery();
                var notifications = await _mediator.Send(query);
                _logger.LogInformation("Se obtuvieron {Count} notificaciones exitosamente.", notifications.Count);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener todas las notificaciones: {Message}", ex.Message);
                return StatusCode(500, "Ocurrió un error interno al obtener las notificaciones.");
            }
        }

        /// <summary>
        /// Obtiene una notificación específica por su ID.
        /// </summary>
        /// <param name="id">El ID de la notificación a buscar.</param>
        /// <returns>La notificación encontrada.</returns>
        /// <response code="200">Retorna la notificación solicitada.</response>
        /// <response code="404">Si la notificación con el ID especificado no se encuentra.</response>
        /// <response code="500">Si ocurre un error interno del servidor.</response>
        [HttpGet("get-notification/{id}")]
        public async Task<IActionResult> GetNotification(Guid id)
        {
            _logger.LogInformation("Inicio de la obtención de notificación por ID: {NotificationId}", id);
            try
            {
                var query = new GetNotificationQuery(id);
                var notification = await _mediator.Send(query);
                _logger.LogInformation("Notificación con ID {NotificationId} obtenida exitosamente.", id);
                return Ok(notification);
            }
            catch (NotificationNotFoundException ex)
            {
                _logger.LogWarning(ex, "Notificación con ID {NotificationId} no encontrada: {Message}", id, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener notificación con ID {NotificationId}: {Message}", id, ex.Message);
                return StatusCode(500, "Ocurrió un error interno al obtener la notificación.");
            }
        }
    }
}


