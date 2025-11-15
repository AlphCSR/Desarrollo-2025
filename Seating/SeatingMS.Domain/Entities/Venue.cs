using SeatingMS.Domain.Entities;

namespace SeatingMS.Domain.Entities
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

    public class Venue : Base
    {
        public string Name { get; set; } = string.Empty; // Ej: "Teatro Principal"
        public string OrganizerId { get; set; } = string.Empty; // Dueño de la plantilla
        public int TotalCapacity { get; set; }

        // Navegación a los asientos de la plantilla
        public ICollection<SeatTemplate> SeatTemplates { get; set; } = new List<SeatTemplate>();
    }
}