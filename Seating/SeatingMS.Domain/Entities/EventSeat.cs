using SeatingMS.Commons.Enums;

namespace SeatingMS.Domain.Entities
{
    public class EventSeat : Base
    {
        public Guid EventId { get; set; } // FK (lógica) al Evento de EventsMS
        public Guid VenueId { get; set; } // FK a la plantilla Venue
        public Guid SeatTemplateId { get; set; } // FK a la plantilla SeatTemplate

        // Datos "cacheados" de la plantilla para rapidez
        public string Row { get; set; } = string.Empty;
        public string SeatNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        // --- Estado en Tiempo Real ---
        public SeatStatus Status { get; set; }
        public decimal Price { get; set; } // El precio puede variar por evento
        public string? LockedByUserId { get; set; } // Quién lo tiene bloqueado
        public DateTime? LockExpiresAt { get; set; } // TTL

        public SeatTemplate SeatTemplate { get; set; } = null!;
    }
}