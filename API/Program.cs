using API.Extensions;
using API.Helpers;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerAndVersioning();

builder.Services.AddDbContextPool<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection_Dev")));

builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

using var serviceScope = app.Services.CreateScope();
var services = serviceScope.ServiceProvider;

try
{
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger<Program>();
    if(app.Environment.IsDevelopment()) 
    {
        logger.LogError(ex, "An error ocurred in Program.cs");
    }
    else
    {
        logger.LogError("Internal Server Error");
    }
}

// Configure the HTTP request pipeline.
app.UseExceptionMiddleware();

app.UseSwaggerAndVersioning(services);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
public partial class Program { }
