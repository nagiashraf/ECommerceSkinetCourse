using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests
{
    public class TestDatabaseFixture
    {
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=ECommerceSkinet_Test;Trusted_Connection=True";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public TestDatabaseFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public StoreContext CreateContext()
            => new StoreContext(
                new DbContextOptionsBuilder<StoreContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);
    }

}