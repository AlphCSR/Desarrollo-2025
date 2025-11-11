using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsMS.Infrastructure.Exceptions
{
    public class NotificationNotFoundException : Exception
    {
        public NotificationNotFoundException() { }

        public NotificationNotFoundException(string message)
            : base(message) { }

        public NotificationNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}
