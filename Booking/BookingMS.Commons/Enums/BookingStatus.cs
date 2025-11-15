namespace BookingMS.Commons.Enums
{
    public enum BookingStatus
    {
        Pending,    // La reserva está activa, esperando pago
        Confirmed,  // Pagada
        Cancelled,  // Cancelada por el usuario
        Expired     // Cancelada por el sistema (tiempo agotado)
    }
}