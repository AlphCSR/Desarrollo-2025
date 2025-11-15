namespace BookingMS.Commons.Events
{
    // Notifica a otros servicios que se ha creado una reserva pendiente.
    public record BookingCreatedEvent
    {
        public Guid BookingId { get; init; }
        public string UserId { get; init; } = string.Empty;
        public Guid EventId { get; init; }
        public decimal Price { get; init; }
        public DateTime ExpiresAt { get; init; }
    }
}