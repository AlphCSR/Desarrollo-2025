using SeatingMS.Domain.Entities;

namespace SeatingMS.Core.Repositories
{
    public interface IEventSeatRepository
    {
        Task<EventSeat?> GetByIdAsync(Guid eventSeatId);
        Task<List<EventSeat>> GetByEventIdAsync(Guid eventId);
        Task UpdateAsync(EventSeat eventSeat);
        Task AddRangeAsync(IEnumerable<EventSeat> eventSeats);
    }
}