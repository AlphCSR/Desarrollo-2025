using SeatingMS.Commons.Enums;

namespace SeatingMS.Commons.Dtos.Response
{
    public record EventSeatMapDto
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; } = string.Empty; // Podemos cachear esto
        public List<SeatStatusDto> Seats { get; set; } = new List<SeatStatusDto>();
    }

    public record SeatStatusDto
    {
        public Guid EventSeatId { get; set; }
        public string Row { get; set; } = string.Empty;
        public string SeatNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public SeatStatus Status { get; set; }
    }
}