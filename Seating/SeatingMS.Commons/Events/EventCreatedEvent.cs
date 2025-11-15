using System;

namespace SeatingMS.Commons.Events
{
    public record EventCreatedEvent
    {
        public Guid EventId { get; init; }
        public string Name { get; init; } = string.Empty;
        public int Capacity { get; init; }
        public Guid VenueId { get; init; } // <-- EventsMS debe decir qué Venue usar.
        public decimal DefaultPrice { get; init; }
    }
}
