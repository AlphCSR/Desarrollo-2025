using MediatR;
using EventsMS.Commons.Dtos.Request;
using EventsMS.Commons.Dtos.Response;

namespace EventsMS.Application.Commands
{
    public class CreateEventCommand : IRequest<EventDto>
    {
        public CreateEventDto CreateEventDto { get; }
        public string OrganizerId { get; } // Lo obtendremos del Token

        public CreateEventCommand(CreateEventDto dto, string organizerId)
        {
            CreateEventDto = dto;
            OrganizerId = organizerId;
        }
    }
}