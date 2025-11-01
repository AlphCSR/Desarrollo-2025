using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Commons.Dtos.Request;

namespace UsersMS.Application.Commands
{
    public class DeleteUserCommand : IRequest<string>
    {
        public DeleteUserDto DeleteUserDto { get; set; }

        public DeleteUserCommand(DeleteUserDto dto)
        {
            DeleteUserDto = dto;
        }
    }
}
