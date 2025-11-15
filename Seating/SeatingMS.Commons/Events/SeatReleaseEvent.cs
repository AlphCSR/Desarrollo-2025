namespace SeatingMS.Commons.Events
{
    public record SeatReleasedEvent
    {
        public Guid EventSeatId { get; init; }
        public string Reason { get; init; } = string.Empty; // "Expired", "Cancelled"
    }
}