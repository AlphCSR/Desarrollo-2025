using MassTransit;
using SeatingMS.Core.Repositories;
using SeatingMS.Domain.Entities;
using SeatingMS.Commons.Enums;
using SeatingMS.Commons.Events;

namespace SeatingMS.Infrastructure.Consumers
{


    public class EventCreatedConsumer : IConsumer<EventCreatedEvent>
    {
        private readonly IEventSeatRepository _seatRepository;
        private readonly IVenueRepository _venueRepository; // Para leer la plantilla

        public EventCreatedConsumer(IEventSeatRepository seatRepository, IVenueRepository venueRepository)
        {
            _seatRepository = seatRepository;
            _venueRepository = venueRepository;
        }

        public async Task Consume(ConsumeContext<EventCreatedEvent> context)
        {
            var message = context.Message;

            // 1. Cargar la plantilla de asientos del Venue
            var venueTemplate = await _venueRepository.GetByIdWithSeatsAsync(message.VenueId);
            if (venueTemplate == null)
            {
                // Loggear error, el VenueId no existe
                return;
            }

            // 2. Generar los asientos "EventSeat" para este evento
            var eventSeats = new List<EventSeat>();
            foreach (var seatTpl in venueTemplate.SeatTemplates)
            {
                eventSeats.Add(new EventSeat
                {
                    Id = Guid.NewGuid(),
                    EventId = message.EventId,
                    VenueId = venueTemplate.Id,
                    SeatTemplateId = seatTpl.Id,
                    Row = seatTpl.Row,
                    SeatNumber = seatTpl.SeatNumber,
                    Type = seatTpl.Type,
                    Status = SeatStatus.Available, // Listos para vender
                    Price = message.DefaultPrice // Precio base
                });
            }

            // 3. Guardar todos los asientos en la BD
            await _seatRepository.AddRangeAsync(eventSeats);
            // (El SaveChangesAsync es manejado por el repositorio)
        }
    }
}