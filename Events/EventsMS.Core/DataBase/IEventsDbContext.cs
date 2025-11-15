using Microsoft.EntityFrameworkCore;
using EventsMS.Domain.Entities;

namespace EventsMS.Core.DataBase
{
    public interface IDbContextTransactionProxy : IDisposable
    {
        void Commit();
        void Rollback();
    }

    public interface IEventsDbContext
    {
        DbContext DbContext { get; }
        DbSet<Event> Events { get; set; }
        DbSet<OutboxMessage> OutboxMessages { get; set; } // Para el Outbox

        IDbContextTransactionProxy BeginTransaction();
        Task<bool> SaveEfContextChanges(string user, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}