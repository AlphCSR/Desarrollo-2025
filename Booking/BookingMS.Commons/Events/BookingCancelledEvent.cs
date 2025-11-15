namespace BookingMS.Commons.Events
{
    // SeatingMS lo escucha para volver a poner el asiento como "Disponible".
    public record BookingCancelledEvent
    {
        public Guid BookingId { get; init; }
        public Guid EventSeatId { get; init; }
        public string Reason { get; init; } = string.Empty; // "Expired", "UserCancelled"
    }
}