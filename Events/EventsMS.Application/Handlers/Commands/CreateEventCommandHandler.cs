using EventsMS.Commons.Dtos.Response;
using EventsMS.Core.Repositories;
using EventsMS.Core.DataBase;
using EventsMS.Domain.Entities;
using EventsMS.Commons.Enums;
using EventsMS.Commons.IntegrationEvents;

namespace EventsMS.Application.Handlers.Commands
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDto>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventsDbContext _context;
        private readonly IValidator<CreateEventDto> _validator;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateEventCommandHandler(
            IEventRepository eventRepository,
            IEventsDbContext context,
            IValidator<CreateEventDto> validator,
            IPublishEndpoint publishEndpoint)
        {
            _eventRepository = eventRepository;
            _context = context;
            _validator = validator;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request.CreateEventDto, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidatorException(validationResult.Errors);

            var dto = request.CreateEventDto;

            var eventEntity = new Event
            {
                Title = dto.Title!,
                Description = dto.Description!,
                Location = dto.Location!,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Category = dto.Category!,
                Capacity = dto.Capacity,
                OrganizerId = request.OrganizerId,
                Status = EventStatus.Draft,
                ImageUrl = dto.ImageUrl
            };

            await _eventRepository.AddAsync(eventEntity);
            await _context.SaveChangesAsync("EventCreated", cancellationToken);

            var eventDto = new EventDto
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                Description = eventEntity.Description,
                Location = eventEntity.Location,
                StartDate = eventEntity.StartDate,
                EndDate = eventEntity.EndDate,
                Category = eventEntity.Category,
                Capacity = eventEntity.Capacity,
                OrganizerId = eventEntity.OrganizerId,
                Status = eventEntity.Status,
                ImageUrl = eventEntity.ImageUrl
            };

            var integrationEvent = new EventCreatedIntegrationEvent
            {
                EventId = eventEntity.Id
            };

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);

            return eventDto;
        }
    }
}