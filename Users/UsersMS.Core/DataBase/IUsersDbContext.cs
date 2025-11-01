using Azure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Domain.Entities;
namespace UsersMS.Core.DataBase
{
    public interface IUsersDbContext
    {
        DbContext DbContext { get; }

        DbSet<User> Users { get; set; }
       // DbSet<Bidder> Bidders { get; set; }
       // DbSet<TechnicalSupport> TechnicalSupports { get; set; }
       // DbSet<Auctioneer> Auctioneers { get; set; }

        IDbContextTransactionProxy BeginTransaction();

        void ChangeEntityState<TEntity>(TEntity entity, EntityState state);

        Task<bool> SaveEfContextChanges(string user, CancellationToken cancellationToken = default);
    }
}
