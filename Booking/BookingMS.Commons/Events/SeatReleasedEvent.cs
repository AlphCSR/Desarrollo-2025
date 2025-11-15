namespace BookingMS.Commons.Events
{
    // Notifica a otros servicios que se ha creado una reserva pendiente.
    public record SeatReleasedEvent
    {
        public Guid EventSeatId { get; init; }
        public string Reason { get; init; } = string.Empty;
    }
}