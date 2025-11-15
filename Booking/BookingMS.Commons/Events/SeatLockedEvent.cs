namespace BookingMS.Commons.Events
{
    // Notifica a otros servicios que se ha creado una reserva pendiente.
    public record SeatLockedEvent
    {
        public Guid EventSeatId { get; init; }
        public Guid EventId { get; init; }
        public string UserId { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public DateTime LockExpiresAt { get; init; }
    }
}