using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsMS.Domain.Entities;

namespace NotificationsMS.Core.Repositories
{
    public interface INotificationRepository
    {   
        Task<Notification?> GetByIdAsync(Guid notificationId);
        Task<List<Notification>> GetAllAsync();
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
    }
}
