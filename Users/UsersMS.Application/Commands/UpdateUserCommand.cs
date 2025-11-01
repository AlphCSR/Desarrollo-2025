using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Commons.Dtos.Request;


namespace UsersMS.Application.Commands
{
    public class UpdateUserCommand : IRequest<string>
    {
        public UpdateUserDto UpdateUserDto { get; set; }

        public UpdateUserCommand(UpdateUserDto dto)
        {
            UpdateUserDto = dto;
        }
    }
}
