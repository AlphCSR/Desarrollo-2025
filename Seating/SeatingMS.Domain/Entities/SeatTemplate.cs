namespace SeatingMS.Domain.Entities
{
    public class SeatTemplate : Base
    {
        public Guid VenueId { get; set; } // FK a Venue
        public string Row { get; set; } = string.Empty; // "A", "B", "General"
        public string SeatNumber { get; set; } = string.Empty; // "1", "2"
        public string Type { get; set; } = string.Empty; // "VIP", "Standard"

        public Venue Venue { get; set; } = null!;
    }
}