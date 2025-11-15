namespace EventsMS.Commons.Dtos.Request
{
    public record CreateEventDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Category { get; set; }
        public int Capacity { get; set; }
        public string? ImageUrl { get; set; }
    }
}