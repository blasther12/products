using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Products.Api.Endpoints;
using Products.Infrastructure.ExceptionHandling;
using Products.Service.DTOs;
using Products.Service.Interfaces;

namespace Products.Tests;

public class ProductsTest
{
    [Fact]
    public async Task GetProductReturnsNotFoundIfNotExists()
    {
        var mock = new Mock<IProductService>();

        mock.Setup(m => m.GetById(It.Is<long>(id => id == 1)))
            .ReturnsAsync((ProductReadDto?)null);

        var result = await ProductsEndpoints.GetProduct(mock.Object, 1);

        Assert.IsType<Results<Ok<ProductReadDto>, NotFound>>(result);

        var notFoundResult = (NotFound)result.Result;

        Assert.NotNull(notFoundResult);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetProductFromDatabase()
    {
        var mock = new Mock<IProductService>();

        mock.Setup(m => m.GetById(It.Is<long>(id => id == 1)))
            .ReturnsAsync(new ProductReadDto { Id = 1, });

        var result = await ProductsEndpoints.GetProduct(mock.Object, 1);

        Assert.IsType<Results<Ok<ProductReadDto>, NotFound>>(result);

        var okResult = (Ok<ProductReadDto>)result.Result;

        Assert.NotNull(okResult.Value);
        Assert.Equal(200, okResult.StatusCode);
        Assert.True(okResult.Value.Id == 1);
    }

    [Fact]
    public async Task GetAllReturnsProductFromDatabase()
    {
        var mock = new Mock<IProductService>();

        mock.Setup(m => m.ListRecords(null, null))
            .ReturnsAsync(new ProductListDto
            {
                TotalCount = 2,
                Products = [
                    new ProductReadDto { Id = 1,},
                    new ProductReadDto { Id = 2,}
                ]
            });

        var result = await ProductsEndpoints.GetAllProducts(mock.Object, null, null);

        Assert.IsType<Results<Ok<ProductListDto>, NotFound<string>>>(result);

        var okResult = (Ok<ProductListDto>)result.Result;

        Assert.NotNull(okResult.Value);
        Assert.Equal(200, okResult.StatusCode);
        Assert.True(okResult.Value.TotalCount == 2);
    }

    [Fact]
    public async Task GetAllReturnsNotFound()
    {
        var mock = new Mock<IProductService>();

        mock.Setup(m => m.ListRecords(null, null))
            .ReturnsAsync(new ProductListDto
            {
                TotalCount = 0,
                Products = []
            });

        var result = await ProductsEndpoints.GetAllProducts(mock.Object, null, null);

        Assert.IsType<Results<Ok<ProductListDto>, NotFound<string>>>(result);

        var notFoundResult = (NotFound<string>)result.Result;

        Assert.NotNull(notFoundResult);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task CreatesProductInDatabase()
    {
        var productDto = new ProductCreateDto
        {
            Name = "Product 1",
            Description = "",
            Quantity = 10,
            Value = 99.99m
        };

        var mock = new Mock<IProductService>();

        mock.Setup(m => m.Register(productDto))
            .Returns(Task.CompletedTask);

        var result = await ProductsEndpoints.RegisterProduct(mock.Object, productDto);

        Assert.IsType<Created>(result);

        var createdResult = (Created)result;

        Assert.NotNull(createdResult);
        Assert.Equal(201, createdResult.StatusCode);
    }

    [Fact]
    public async Task CreatesProductReturnsBadRequest()
    {
        var productDto = new ProductCreateDto
        {
            Name = "Product 1",
            Description = "",
            Quantity = 10,
            Value = -99.99m
        };

        var mock = new Mock<IProductService>();

        mock.Setup(m => m.Register(productDto))
            .Throws(new ValidationException("The product value cannot be less than zero!"));

        var result = await ProductsEndpoints.RegisterProduct(mock.Object, productDto);

        Assert.IsType<BadRequest<string>>(result);

        var badRequestResult = (BadRequest<string>)result;

        Assert.NotNull(badRequestResult);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("The product value cannot be less than zero!", badRequestResult.Value);
    }

    [Fact]
    public async Task CreatesProductReturnsDuplicateException()
    {
        var productDto = new ProductCreateDto
        {
            Name = "Product 1",
            Description = "",
            Quantity = 10,
            Value = 99.99m
        };

        var mock = new Mock<IProductService>();

        mock.Setup(m => m.Register(productDto))
            .Throws(new Microsoft.EntityFrameworkCore.DbUpdateException("", new Exception("unique constraint")));

        var result = await ProductsEndpoints.RegisterProduct(mock.Object, productDto);

        Assert.IsType<Conflict<string>>(result);

        var conflictResult = (Conflict<string>)result;

        Assert.NotNull(conflictResult);
        Assert.Equal(409, conflictResult.StatusCode);
    }

    [Fact]
    public async Task CreatesProductReturnsException()
    {
        var productDto = new ProductCreateDto
        {
            Name = "Product 1",
            Description = "",
            Quantity = 10,
            Value = 99.99m
        };

        var mock = new Mock<IProductService>();

        mock.Setup(m => m.Register(productDto))
            .Throws(new Exception());

        var result = await ProductsEndpoints.RegisterProduct(mock.Object, productDto);

        Assert.IsType<StatusCodeHttpResult>(result);

        var statusCodeHttpResult = (StatusCodeHttpResult)result;

        Assert.NotNull(statusCodeHttpResult);
        Assert.Equal(500, statusCodeHttpResult.StatusCode);
    }

    [Fact]
    public async Task UpdateProductInDatabase()
    {
        var productDto = new ProductUpdateDto
        {
            Id = 1,
            Name = "Product 1",
            Description = "",
            Quantity = 10,
            Value = 99.99m
        };

        var mock = new Mock<IProductService>();

        mock.Setup(m => m.Update(productDto))
            .Returns(Task.CompletedTask);

        var result = await ProductsEndpoints.UpdateProduct(mock.Object, productDto);

        Assert.IsType<NoContent>(result);

        var noContentResult = (NoContent)result;

        Assert.NotNull(noContentResult);
        Assert.Equal(204, noContentResult.StatusCode);
    }

    [Fact]
    public async Task UpdateProductReturnsBadRequest()
    {
        var productDto = new ProductUpdateDto
        {
            Id = 1,
            Name = "Product 1",
            Description = "",
            Quantity = 10,
            Value = -99.99m
        };

        var mock = new Mock<IProductService>();

        mock.Setup(m => m.Update(productDto))
            .Throws(new ValidationException("The product value cannot be less than zero!"));

        var result = await ProductsEndpoints.UpdateProduct(mock.Object, productDto);

        Assert.IsType<BadRequest<string>>(result);

        var badRequestResult = (BadRequest<string>)result;

        Assert.NotNull(badRequestResult);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("The product value cannot be less than zero!", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateProductReturnsException()
    {
        var productDto = new ProductUpdateDto
        {
            Id = 1,
            Name = "Product 1",
            Description = "",
            Quantity = 10,
            Value = 99.99m
        };

        var mock = new Mock<IProductService>();

        mock.Setup(m => m.Update(productDto))
            .Throws(new Exception());

        var result = await ProductsEndpoints.UpdateProduct(mock.Object, productDto);

        Assert.IsType<StatusCodeHttpResult>(result);

        var statusCodeHttpResult = (StatusCodeHttpResult)result;

        Assert.NotNull(statusCodeHttpResult);
        Assert.Equal(500, statusCodeHttpResult.StatusCode);
    }

    [Fact]
    public async Task DeleteProductInDatabase()
    {
        var mock = new Mock<IProductService>();

        mock.Setup(m => m.Delete(1))
            .Returns(Task.CompletedTask);

        var result = await ProductsEndpoints.DeleteProduct(mock.Object, 1);

        Assert.IsType<NoContent>(result);

        var noContentResult = (NoContent)result;

        Assert.NotNull(noContentResult);
        Assert.Equal(204, noContentResult.StatusCode);
    }

    [Fact]
    public async Task DeleteProductReturnsNotFound()
    {
        var mock = new Mock<IProductService>();

        mock.Setup(m => m.Delete(1))
            .Throws(new NotFoundException("No data found for deletion!"));

        var result = await ProductsEndpoints.DeleteProduct(mock.Object, 1);

        Assert.IsType<NotFound<string>>(result);

        var notFoundResult = (NotFound<string>)result;

        Assert.NotNull(notFoundResult);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal("No data found for deletion!", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteProductReturnsException()
    {
        var mock = new Mock<IProductService>();

        mock.Setup(m => m.Delete(1))
            .Throws(new Exception());

        var result = await ProductsEndpoints.DeleteProduct(mock.Object, 1);

        Assert.IsType<StatusCodeHttpResult>(result);

        var statusCodeHttpResult = (StatusCodeHttpResult)result;

        Assert.NotNull(statusCodeHttpResult);
        Assert.Equal(500, statusCodeHttpResult.StatusCode);
    }
}