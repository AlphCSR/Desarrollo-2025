using Microsoft.EntityFrameworkCore;
using SeatingMS.Domain.Entities;

namespace SeatingMS.Core.DataBase
{
    public interface IDbContextTransactionProxy : IDisposable
    {
        void Commit();
        void Rollback();
    }

    public interface ISeatingDbContext
    {
        DbContext DbContext { get; }
        DbSet<Venue> Venues { get; set; }
        DbSet<SeatTemplate> SeatTemplates { get; set; }
        DbSet<EventSeat> EventSeats { get; set; }
        DbSet<OutboxMessage> OutboxMessages { get; set; }

        IDbContextTransactionProxy BeginTransaction();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}