using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Infrastructure;
using Products.Infrastructure.ExceptionHandling;
using Products.Infrastructure.Repositories;
using Products.Tests.Helpers;

namespace Products.Tests
{
    public class ProductsRepositoryTest
    {
        [Fact]
        public async Task GetProductFromDatabase()
        {
            await using var context = new MockDb().CreateDbContext();

            context.Product.Add(new Product
            {
                Id = 1,
                Name = "Product 1",
                Description = "",
                Quantity = 10,
                Value = 99.99m
            });

            await context.SaveChangesAsync();

            var result = await new Repository<Product>(context).GetById(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetProductsByNameFromDatabase()
        {
            await using var context = new MockDb().CreateDbContext();

            await context.Product.AddRangeAsync([
                new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "",
                    Quantity = 10,
                    Value = 99.99m
                },
                new Product
                {
                    Id = 2,
                    Name = "Product 2",
                    Description = "",
                    Quantity = 10,
                    Value = 99.99m
                }
            ]);

            await context.SaveChangesAsync();

            var result = await new ProductRepository(context).ListRecords("Product 2", null);

            Assert.NotNull(result);
            Assert.True(result.Count == 1);
        }

        [Fact]
        public async Task GetProductsSortedByDateFromDatabase()
        {
            await using var context = new MockDb().CreateDbContext();

            await context.Product.AddRangeAsync([
                new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "",
                    Quantity = 10,
                    Value = 99.99m
                },
                new Product
                {
                    Id = 2,
                    Name = "Product 2",
                    Description = "",
                    Quantity = 10,
                    Value = 99.99m
                }
            ]);

            await context.SaveChangesAsync();

            var result = await new ProductRepository(context).ListRecords(null, "date");

            Assert.NotNull(result);
            Assert.True(result.Count == 2);

            Assert.Collection(result, product =>
                {
                    Assert.Equal(1, product.Id);
                }, product2 =>
                {
                    Assert.Equal(2, product2.Id);
                });
        }

        [Fact]
        public async Task GetProductsSortedByValueFromDatabase()
        {
            await using var context = new MockDb().CreateDbContext();

            await context.Product.AddRangeAsync([
                new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "",
                    Quantity = 10,
                    Value = 99.99m
                },
                new Product
                {
                    Id = 2,
                    Name = "Product 2",
                    Description = "",
                    Quantity = 10,
                    Value = 55.99m
                }
            ]);

            await context.SaveChangesAsync();

            var result = await new ProductRepository(context).ListRecords(null, "value");

            Assert.NotNull(result);
            Assert.True(result.Count == 2);

            Assert.Collection(result, product =>
                {
                    Assert.Equal(2, product.Id);
                }, product2 =>
                {
                    Assert.Equal(1, product2.Id);
                });
        }

        [Fact]
        public async Task DeleteProductFromDatabase()
        {
            await using var context = new MockDb().CreateDbContext();

            context.Product.Add(new Product
            {
                Id = 1,
                Name = "Product 1",
                Description = "",
                Quantity = 10,
                Value = 99.99m
            });

            await context.SaveChangesAsync();

            await new Repository<Product>(context).Delete(1);
        }

        [Fact]
        public async Task DeleteNonExistentProductFromDatabase()
        {
            await using var context = new MockDb().CreateDbContext();

            context.Product.Add(new Product
            {
                Id = 1,
                Name = "Product 1",
                Description = "",
                Quantity = 10,
                Value = 99.99m
            });

            await context.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                new Repository<Product>(context).Delete(2));

            Assert.Equal("No data found for deletion!", exception.Message);
        }

        [Fact]
        public async Task CreateProductFromDatabase()
        {
            await using var context = new MockDb().CreateDbContext();

            await new Repository<Product>(context).Register(new Product
            {
                Name = "Product 1",
                Description = "",
                Quantity = 10,
                Value = 99.99m
            });

            var productEntity = await context.Product.FirstOrDefaultAsync();

            Assert.NotNull(productEntity);
            Assert.Equal(1, productEntity.Id);
        }

        [Fact]
        public async Task UpdateProductFromDatabase()
        {
            await using var context = new MockDb().CreateDbContext();

            context.Product.Add(new Product
            {
                Id = 1,
                Name = "Product 1",
                Description = "",
                Quantity = 10,
                Value = 99.99m
            });

            await context.SaveChangesAsync();

            var existingProduct = await context.Product.FirstOrDefaultAsync();
            context.Entry(existingProduct!).State = EntityState.Detached;

            await new Repository<Product>(context).Update(new Product
            {
                Id = 1,
                Name = "Product Test",
                Description = "test",
                Quantity = 15,
                Value = 100.99m
            });

            var productEntity = await context.Product.FirstOrDefaultAsync();

            Assert.NotNull(productEntity);
            Assert.Equal(1, productEntity.Id);
            Assert.Equal(15, productEntity.Quantity);
            Assert.Equal(100.99m, productEntity.Value);
            Assert.Equal("test", productEntity.Description);
        }
    }
}