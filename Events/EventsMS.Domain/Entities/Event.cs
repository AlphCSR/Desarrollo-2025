using EventsMS.Commons.Enums;
using EventsMS.Domain.Entities;

namespace EventsMS.Domain.Entities
{
    public class Base
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class Event : Base
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Capacity { get; set; } // Aforo total 
        public string OrganizerId { get; set; } = string.Empty; // ID del User (Keycloak o DB) que organiza el evento
        public EventStatus Status { get; set; }
        public string? ImageUrl { get; set; } // URL al archivo en Firebase/Blob 

        public Event() {}

        public Event(string name, string description, string location, DateTime startDate, DateTime endDate, string category, int capacity, string organizerId, EventStatus status, string? imageUrl = null)
        {
            Id = Guid.NewGuid(); 
            Name = name;
            Description = description;
            Location = location;
            StartDate = startDate;
            EndDate = endDate;
            Category = category;
            Capacity = capacity;
            OrganizerId = organizerId;
            Status = status;
            ImageUrl = imageUrl;
            CreatedAt = DateTime.UtcNow;
        }
    }
}