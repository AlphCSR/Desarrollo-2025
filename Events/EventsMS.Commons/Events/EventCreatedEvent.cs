namespace EventsMS.Commons.IntegrationEvents
{
    public record EventCreatedIntegrationEvent
    {
        public Guid EventId { get; init; }
        public string Name { get; init; } = string.Empty;
        public int Capacity { get; init; }
        public DateTime StartDate { get; init; }
    }
}