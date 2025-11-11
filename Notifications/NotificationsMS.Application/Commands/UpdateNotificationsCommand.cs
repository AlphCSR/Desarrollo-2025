using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsMS.Commons.Dtos.Request;
using NotificationsMS.Domain.Entities;

namespace NotificationsMS.Application.Commands
{
    public class UpdateNotificationCommand : IRequest<string>
    {
        public UpdateNotificationDto UpdateNotificationDto { get; set; }

        public UpdateNotificationCommand(UpdateNotificationDto updateNotificationDto)
        {
            UpdateNotificationDto = updateNotificationDto;
        }
    }
}