using EventsMS.Domain.Entities;

namespace EventsMS.Core.Repositories
{
    public interface IEventRepository
    {
        Task<Event?> GetByIdAsync(Guid eventId);
        Task<List<Event>> GetAllPublishedAsync();
        Task<List<Event>> GetByOrganizerAsync(string organizerId);
        Task AddAsync(Event eventEntity);
        Task UpdateAsync(Event eventEntity);
    }
}