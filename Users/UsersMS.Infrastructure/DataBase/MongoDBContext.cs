using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using UsersMS.Domain.Entities;

namespace UsersMS.Infrastructure.DataBase
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDb:ConnectionString"];
            var dbName = configuration["MongoDb:Database"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
        }

        public MongoDbContext() { }

        public virtual IMongoCollection<UserReadModel> Users => _database.GetCollection<UserReadModel>("Users");
    }
}
