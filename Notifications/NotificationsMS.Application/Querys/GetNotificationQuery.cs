using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsMS.Commons.Dtos.Response;

namespace NotificationsMS.Application.Querys
{
    public class GetNotificationQuery : IRequest<GetNotificationDto>
    {
        public Guid IdNotification { get; set; }
        
        public GetNotificationQuery() {}

        public GetNotificationQuery(Guid idNotification)
        {
            IdNotification = idNotification;
        }
    }
}