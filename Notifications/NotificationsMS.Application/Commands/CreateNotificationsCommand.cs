using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsMS.Commons.Dtos.Request;

namespace NotificationsMS.Application.Commands
{
    public class CreateNotificationCommand : IRequest<string>
    {
        public CreateNotificationDto CreateNotificationDto { get; set; }

        public CreateNotificationCommand(CreateNotificationDto createNotificationDto)
        {
            CreateNotificationDto = createNotificationDto;
        }
    }
}

