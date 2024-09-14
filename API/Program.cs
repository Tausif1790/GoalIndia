using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IProductRepository, ProductRepository>();             // live till http req

// before this line everything is considered as services
var app = builder.Build();
// after this line everything is considered is middleware

app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();       // Creating a scope for the application's services (dependency injection)
    var services = scope.ServiceProvider;               // Accessing the service provider to resolve dependencies
    var context = services.GetRequiredService<StoreContext>(); // Getting the required StoreContext service (DB context)
    await context.Database.MigrateAsync();              // Applying any pending database migrations asynchronously
    await StoreContextSeed.SeedAsync(context);          // Seeding the database with initial data if necessary
}
catch (Exception ex)
{
    Console.WriteLine(ex);                              // Logging the exception details to the console
    throw;                                              // Rethrowing the exception to ensure the error is not swallowed
}


app.Run();
