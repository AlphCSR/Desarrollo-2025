using BookingMS.Commons.Enums;
using BookingMS.Domain.Entities;

namespace BookingMS.Domain.Entities
{
    // Usamos la misma clase Base que tienes en UsersMS
    public class Base
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class Booking : Base
    {
        public string UserId { get; set; } = string.Empty;
        public Guid EventId { get; set; }
        public Guid EventSeatId { get; set; } // El asiento específico reservado
        
        public decimal Price { get; set; }
        public DateTime ExpiresAt { get; set; } // Cuándo expira (info de SeatingMS)
        public BookingStatus Status { get; set; }
    }
}