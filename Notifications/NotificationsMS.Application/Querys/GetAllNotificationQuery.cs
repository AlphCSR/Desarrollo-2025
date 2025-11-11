using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsMS.Commons.Dtos.Response;

namespace NotificationsMS.Application.Querys
{
    public class GetAllNotificationQuery : IRequest<List<GetAllNotificationDto>>
    {
        public GetAllNotificationQuery() { }
    }
}