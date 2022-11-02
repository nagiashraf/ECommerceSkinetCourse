using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace API.Tests
{
    public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(async services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType ==typeof(DbContextOptions<StoreContext>));
                if (descriptor is not null)
                    services.Remove(descriptor);
                services.AddDbContextPool<StoreContext>((options, context) => 
                {
                    context.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ECommerceSkinet_Test;Trusted_Connection=True");
                });
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var appContext = scopedServices.GetRequiredService<StoreContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<WebApplicationFactory<Program>>>();
                    await appContext.Database.EnsureCreatedAsync();
                    try
                    {
                        await StoreContextSeed.SeedAsync(appContext);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database with test data. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}