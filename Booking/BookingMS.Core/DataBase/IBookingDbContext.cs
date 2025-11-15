using Microsoft.EntityFrameworkCore;
using BookingMS.Domain.Entities;
    
namespace BookingMS.Core.DataBase
{
    public interface IDbContextTransactionProxy : IDisposable
    {
        void Commit();
        void Rollback();
    }

    public interface IBookingDbContext
    {
        DbContext DbContext { get; }
        DbSet<Booking> Bookings { get; set; }
        DbSet<OutboxMessage> OutboxMessages { get; set; }

        IDbContextTransactionProxy BeginTransaction();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}