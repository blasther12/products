using Microsoft.EntityFrameworkCore;
using Products.Api.Endpoints;
using Products.Infrastructure;
using Products.Infrastructure.Interfaces;
using Products.Infrastructure.Repositories;
using Products.Service;
using Products.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductsDbContext>();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    if(dbContext != null)
    {
        if(dbContext.Database.EnsureCreated()){
            
        }
        else if(dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
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

app.MapWeatherforecastEndpoints();
app.MapHealthChecks("/healthz");

app.Run();