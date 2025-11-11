using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsMS.Core.Messaging.Sender
{
    public interface IEmail
    {
        Task SendEmailAsync(string email, string body);
    }
}