using Microsoft.EntityFrameworkCore;
using EventsMS.Core.Repositories;
using EventsMS.Domain.Entities;
using EventsMS.Infrastructure.DataBase;

namespace EventsMS.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventsDbContext _dbContext;

        public EventRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Event eventEntity)
        {
            await _dbContext.Events.AddAsync(eventEntity);
        }

        public async Task<Event?> GetByIdAsync(Guid eventId)
        {
            return await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId);
        }

        public async Task<List<Event>> GetAllPublishedAsync()
        {
            return await _dbContext.Events
                .Where(e => e.Status == Commons.Enums.EventStatus.Published)
                .OrderBy(e => e.StartDate)
                .ToListAsync();
        }

        public async Task<List<Event>> GetByOrganizerAsync(string organizerId)
        {
            return await _dbContext.Events
                .Where(e => e.OrganizerId == organizerId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(Event eventEntity)
        {
            _dbContext.Events.Update(eventEntity);
        }
    }
}