using Moq;
using Products.Domain.Entities;
using Products.Infrastructure.ExceptionHandling;
using Products.Infrastructure.Interfaces;
using Products.Service;
using Products.Service.DTOs;

namespace Products.Tests
{
    public class ProductInMemoryTests
    {
        [Fact]
        public async Task GetProductReturnsNullIfNotExists()
        {
            var mock = new Mock<IProductRepository>();

            mock.Setup(m => m.GetById(It.Is<long>(id => id == 1)))
                .ReturnsAsync((Product?)null);

            var result = await new ProductService(mock.Object).GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductFromDatabase()
        {
            var mock = new Mock<IProductRepository>();

            mock.Setup(m => m.GetById(It.Is<long>(id => id == 1)))
                .ReturnsAsync(new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "",
                    Quantity = 10,
                    Value = 99.99m
                });

            var result = await new ProductService(mock.Object).GetById(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetProductsFromDatabase()
        {
            var mock = new Mock<IProductRepository>();

            mock.Setup(m => m.ListRecords(null, null))
                .ReturnsAsync([
                    new Product
                    {
                        Id = 1,
                        Name = "Product 1",
                        Description = "",
                        Quantity = 10,
                        Value = 99.99m
                    }
                ]);

            var result = await new ProductService(mock.Object).ListRecords(null, null);

            Assert.NotNull(result);
            Assert.True(result.TotalCount > 0);
        }

        [Fact]
        public async Task RegisterProductInDatabase()
        {
            var mock = new Mock<IProductRepository>();

            var product = new Product
            {
                Name = "Product 1",
                Description = "",
                Quantity = 10,
                Value = 99.99m
            };

            var productDto = new ProductCreateDto
            {
                Name = "Product 1",
                Description = "",
                Quantity = 10,
                Value = 99.99m
            };

            mock.Setup(m => m.Register(product))
                .Returns(Task.CompletedTask);

            await new ProductService(mock.Object).Register(productDto);
        }

        [Fact]
        public async Task RegisterProductNameIsEmpty()
        {
            var mock = new Mock<IProductRepository>();

            var productDto = new ProductCreateDto
            {
                Name = "",
                Description = "",
                Quantity = 10,
                Value = 99.99m
            };

            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                new ProductService(mock.Object).Register(productDto));

            Assert.Equal("Name cannot be null!", exception.Message);
        }

        [Fact]
        public async Task RegisterProductValueIsLessThanZero()
        {
            var mock = new Mock<IProductRepository>();

            var productDto = new ProductCreateDto
            {
                Name = "Test",
                Description = "",
                Quantity = 10,
                Value = -99.99m
            };

            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                new ProductService(mock.Object).Register(productDto));

            Assert.Equal("The product value cannot be less than zero!", exception.Message);
        }

        [Fact]
        public async Task UpdateProductInDatabase()
        {
            var mock = new Mock<IProductRepository>();

            var product = new Product
            {
                Id = 1,
                Name = "Product 1",
                Description = "",
                Quantity = 10,
                Value = 99.99m
            };

            var productDto = new ProductUpdateDto
            {
                Id = 1,
                Name = "Product 1",
                Description = "",
                Quantity = 10,
                Value = 99.99m
            };

            mock.Setup(m => m.Update(product))
                .Returns(Task.CompletedTask);

            await new ProductService(mock.Object).Update(productDto);
        }

        [Fact]
        public async Task UpdateProductNameIsEmpty()
        {
            var mock = new Mock<IProductRepository>();

            var productDto = new ProductUpdateDto
            {
                Id = 1,
                Name = "",
                Description = "",
                Quantity = 10,
                Value = 99.99m
            };

            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                new ProductService(mock.Object).Register(productDto));

            Assert.Equal("Name cannot be null!", exception.Message);
        }

        [Fact]
        public async Task UpdateProductValueIsLessThanZero()
        {
            var mock = new Mock<IProductRepository>();

            var productDto = new ProductUpdateDto
            {
                Id = 1,
                Name = "Test",
                Description = "",
                Quantity = 10,
                Value = -99.99m
            };

            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                new ProductService(mock.Object).Register(productDto));

            Assert.Equal("The product value cannot be less than zero!", exception.Message);
        }

        [Fact]
        public async Task DeleteProductInDatabase()
        {
            var mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Delete(1))
                .Returns(Task.CompletedTask);

            await new ProductService(mock.Object).Delete(1);
        }
    }
}