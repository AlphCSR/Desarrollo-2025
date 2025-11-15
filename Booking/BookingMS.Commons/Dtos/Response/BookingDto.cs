using BookingMS.Commons.Enums;

namespace BookingMS.Commons.Dtos.Response
{
    public record BookingDto
    {
        public Guid BookingId { get; set; }
        public Guid EventId { get; set; }
        public Guid EventSeatId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime ExpiresAt { get; set; }
        public BookingStatus Status { get; set; }
        
        // (Opcional) datos cacheados
        // public string EventName { get; set; }
        // public string SeatInfo { get; set; } 
    }
}