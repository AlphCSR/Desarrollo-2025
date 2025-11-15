namespace SeatingMS.Commons.Dtos.Request
{
    public record LockSeatRequestDto
    {
        public Guid EventId { get; set; }
        public Guid EventSeatId { get; set; }
    }
}