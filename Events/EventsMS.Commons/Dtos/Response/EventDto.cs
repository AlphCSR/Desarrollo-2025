using EventsMS.Commons.Enums;

namespace EventsMS.Commons.Dtos.Response
{
    public record EventDto
    {
        public Guid EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string OrganizerId { get; set; } = string.Empty;
        public EventStatus Status { get; set; }
        public string? ImageUrl { get; set; }
    }
}