using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Commons.Dtos.Response;

namespace UsersMS.Application.Queries
{
    public class GetAllUsersQuery : IRequest<List<GetAllUsersDto>>
    {
        public GetAllUsersQuery() { }
    }
}

