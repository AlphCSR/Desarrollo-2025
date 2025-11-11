using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationsMS.Core.Repositories;
using NotificationsMS.Domain.Entities;
using NotificationsMS.Infrastructure.DataBase;
using NotificationsMS.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;

namespace NotificationsMS.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación del repositorio para la entidad <see cref="Notification"/>.
    /// Proporciona métodos para interactuar con la base de datos para operaciones CRUD de notificaciones.
    /// </summary>
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext _dbContext;
        private readonly ILogger<NotificationRepository> _logger;

        /// <summary>
        /// Constructor de <see cref="NotificationRepository"/>.
        /// </summary>
        /// <param name="dbContext">Contexto de la base de datos para notificaciones.</param>
        /// <param name="logger">Instancia de logger para registrar eventos.</param>
        public NotificationRepository(NotificationDbContext dbContext, ILogger<NotificationRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("NotificationRepository instanciado.");
        }

        /// <summary>
        /// Agrega una nueva notificación a la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="notification">La notificación a agregar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <exception cref="ApplicationException">Se lanza si ocurre un error al guardar la notificación en la base de datos.</exception>
        public async Task AddAsync(Notification notification)
        {
            _logger.LogInformation("Intentando agregar notificación con ID: {IdNotification}", notification.IdNotification);
            try
            {
                await _dbContext.Notifications.AddAsync(notification);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Notificación con ID: {IdNotification} agregada exitosamente.", notification.IdNotification);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al agregar notificación con ID: {IdNotification}", notification.IdNotification);
                throw new ApplicationException($"Error al guardar la notificación con ID {notification.IdNotification} en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al agregar notificación con ID: {IdNotification}", notification.IdNotification);
                throw new ApplicationException($"Error inesperado al agregar la notificación con ID {notification.IdNotification}.", ex);
            }
        }

        /// <summary>
        /// Elimina una notificación de la base de datos de forma asíncrona por su ID.
        /// </summary>
        /// <param name="notificationId">El ID de la notificación a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <exception cref="NotificationNotFoundException">Se lanza si la notificación no se encuentra.</exception>
        /// <exception cref="ApplicationException">Se lanza si ocurre un error al eliminar la notificación en la base de datos.</exception>
        public async Task DeleteAsync(Guid notificationId)
        {
            _logger.LogInformation("Intentando eliminar notificación con ID: {IdNotification}", notificationId);
            var notificationEntity = await _dbContext.Notifications.FindAsync(notificationId);
            if (notificationEntity == null)
            {
                _logger.LogWarning("Notificación con ID: {IdNotification} no encontrada para eliminación.", notificationId);
                throw new NotificationNotFoundException($"Notificación con ID {notificationId} no encontrada.");
            }

            try
            {
                _dbContext.Notifications.Remove(notificationEntity);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Notificación con ID: {IdNotification} eliminada exitosamente.", notificationId);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al eliminar notificación con ID: {IdNotification}", notificationId);
                throw new ApplicationException($"Error al eliminar la notificación con ID {notificationId} en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar notificación con ID: {IdNotification}", notificationId);
                throw new ApplicationException($"Error inesperado al eliminar la notificación con ID {notificationId}.", ex);
            }
        }

        /// <summary>
        /// Actualiza una notificación existente en la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="notification">La notificación a actualizar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <exception cref="ApplicationException">Se lanza si ocurre un error al actualizar la notificación en la base de datos.</exception>
        public async Task UpdateAsync(Notification notification)
        {
            _logger.LogInformation("Intentando actualizar notificación con ID: {IdNotification}", notification.IdNotification);
            try
            {
                _dbContext.Notifications.Update(notification);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Notificación con ID: {IdNotification} actualizada exitosamente.", notification.IdNotification);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al actualizar notificación con ID: {IdNotification}", notification.IdNotification);
                throw new ApplicationException($"Error al actualizar la notificación con ID {notification.IdNotification} en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar notificación con ID: {IdNotification}", notification.IdNotification);
                throw new ApplicationException($"Error inesperado al actualizar la notificación con ID {notification.IdNotification}.", ex);
            }
        }

        /// <summary>
        /// Obtiene una notificación por su ID de forma asíncrona.
        /// </summary>
        /// <param name="notificationId">El ID de la notificación a buscar.</param>
        /// <returns>La notificación encontrada, o null si no existe.</returns>
        /// <exception cref="ApplicationException">Se lanza si ocurre un error al buscar la notificación en la base de datos.</exception>
        public async Task<Notification?> GetByIdAsync(Guid notificationId)
        {
            _logger.LogInformation("Intentando obtener notificación con ID: {IdNotification}", notificationId);
            try
            {
                var notification = await _dbContext.Notifications.FirstOrDefaultAsync(u => u.IdNotification == notificationId);
                if (notification == null)
                {
                    _logger.LogInformation("Notificación con ID: {IdNotification} no encontrada.", notificationId);
                }
                else
                {
                    _logger.LogInformation("Notificación con ID: {IdNotification} obtenida exitosamente.", notificationId);
                }
                return notification;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener notificación con ID: {IdNotification}", notificationId);
                throw new ApplicationException($"Error al obtener la notificación con ID {notificationId}.", ex);
            }
        }

        /// <summary>
        /// Obtiene una notificación por el ID de usuario de forma asíncrona.
        /// </summary>
        /// <param name="idUser">El ID del usuario asociado a la notificación a buscar.</param>
        /// <returns>La notificación encontrada, o null si no existe.</returns>
        /// <exception cref="ApplicationException">Se lanza si ocurre un error al buscar la notificación en la base de datos.</exception>
        public async Task<Notification?> GetByUserAsync(Guid idUser)
        {
            _logger.LogInformation("Intentando obtener notificación para el usuario con ID: {IdUser}", idUser);
            try
            {
                var notification = await _dbContext.Notifications.FirstOrDefaultAsync(u => u.IdUser == idUser);
                if (notification == null)
                {
                    _logger.LogInformation("Notificación para el usuario con ID: {IdUser} no encontrada.", idUser);
                }
                else
                {
                    _logger.LogInformation("Notificación para el usuario con ID: {IdUser} obtenida exitosamente.", idUser);
                }
                return notification;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener notificación para el usuario con ID: {IdUser}", idUser);
                throw new ApplicationException($"Error al obtener la notificación para el usuario con ID {idUser}.", ex);
            }
        }

        /// <summary>
        /// Obtiene todas las notificaciones de la base de datos de forma asíncrona.
        /// </summary>
        /// <returns>Una lista de todas las notificaciones.</returns>
        /// <exception cref="ApplicationException">Se lanza si ocurre un error al obtener las notificaciones de la base de datos.</exception>
        public async Task<List<Notification>> GetAllAsync()
        {
            _logger.LogInformation("Intentando obtener todas las notificaciones.");
            try
            {
                var notifications = await _dbContext.Notifications.ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} notificaciones exitosamente.", notifications.Count);
                return notifications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las notificaciones.");
                throw new ApplicationException("Error al obtener todas las notificaciones de la base de datos.", ex);
            }
        }
    }

}
