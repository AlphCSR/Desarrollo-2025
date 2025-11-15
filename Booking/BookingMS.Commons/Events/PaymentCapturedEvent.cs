namespace BookingMS.Commons.Events
{
    // Notifica a otros servicios que se ha creado una reserva pendiente.
    public record PaymentCapturedEvent
    {
        public Guid BookingId { get; init; }
    }

}