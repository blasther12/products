using Microsoft.EntityFrameworkCore;
using Products.Api.Endpoints;
using Products.Infrastructure;
using Products.Infrastructure.DataSeeding;
using Products.Infrastructure.Interfaces;
using Products.Infrastructure.Repositories;
using Products.Service;
using Products.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductsDbContext>(options =>
{
    var configuration = builder.Configuration;
    var connectionString = 
        Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING") ?? 
        configuration.GetConnectionString("DefaultConnection")!.Replace("{PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"));

    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    if(dbContext != null)
    {
        if(dbContext.Database.EnsureCreated()){
            var seeder = new ProductSeeder(dbContext);
            await seeder.SeedAsync();
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("api/products")
    .MapProductsApi()
    .WithTags("Products Endpoints");
app.MapHealthChecks("/healthz");

app.Run();