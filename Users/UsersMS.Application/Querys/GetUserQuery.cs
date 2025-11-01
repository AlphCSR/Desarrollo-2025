using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Commons.Dtos.Response;
using MediatR;

namespace UsersMS.Application.Queries
{
    public class GetUserQuery : IRequest<GetUserDto>
    {
        public Guid UserId { get; set; }

        public GetUserQuery() { }

        public GetUserQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
