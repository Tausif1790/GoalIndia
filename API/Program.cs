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
    using var scope = app.Services.CreateScope();       // 
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);      // 
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

app.Run();
