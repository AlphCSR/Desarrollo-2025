using MediatR;
using SeatingMS.Commons.Dtos.Request;
using SeatingMS.Commons.Dtos.Response;

namespace SeatingMS.Application.Commands
{
    public class LockSeatCommand : IRequest<bool>
    {
        public LockSeatRequestDto LockRequest { get; }
        public string UserId { get; }

        public LockSeatCommand(LockSeatRequestDto dto, string userId)
        {
            LockRequest = dto;
            UserId = userId;
        }
    }
}