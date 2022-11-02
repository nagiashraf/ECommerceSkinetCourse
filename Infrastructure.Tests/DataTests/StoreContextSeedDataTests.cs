using Infrastructure.Data;

namespace Infrastructure.Tests.DataTests
{
    public class StoreContextSeedDataTests : IClassFixture<TestDatabaseFixture>
    {
        public TestDatabaseFixture Fixture { get; }
        public StoreContextSeedDataTests(TestDatabaseFixture fixture) => Fixture = fixture;

        [Fact]
        public async Task SeedDataAsync_SeedCorrectly()
        {
            using var context = Fixture.CreateContext();
            context.Database.BeginTransaction();

            await StoreContextSeed.SeedAsync(context);

            context.ChangeTracker.Clear();

            Assert.True(context.Products.Any());
            Assert.True(context.ProductBrands.Any());
            Assert.True(context.ProductTypes.Any());
        }
    }
}