namespace BookingMS.Commons.Events
{
    // SeatingMS lo escucha para marcar el asiento como "Vendido".
    // NotificationsMS lo escucha para enviar el "Pago Exitoso".
    public record BookingConfirmedEvent
    {
        public Guid BookingId { get; init; }
        public Guid EventSeatId { get; init; }
        public string UserId { get; init; } = string.Empty;
        public Guid EventId { get; init; }
    }
}