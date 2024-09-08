using Microsoft.AspNetCore.Http.HttpResults;
using Products.Infrastructure.ExceptionHandling;
using Products.Service.DTOs;
using Products.Service.Interfaces;

namespace Products.Api.Endpoints
{
    public static class ProductsEndpoints
    {
        public static RouteGroupBuilder MapProductsApi(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", GetProduct)
            .WithName("GetProduct")
            .WithOpenApi();

            group.MapGet("/", GetAllProducts)
            .WithName("GetProducts")
            .WithOpenApi(generatedOperation =>
            {
                var name = generatedOperation.Parameters[0];
                name.Description = "Filter by product name";
                var sortBy = generatedOperation.Parameters[1];
                sortBy.Description = "Sort results. Allowed parameters: ['value', 'date']";
                return generatedOperation;
            });

            group.MapPost("/", RegisterProduct)
            .WithName("PostProduct")
            .WithOpenApi();

            group.MapPut("/", UpdateProduct)
            .WithName("PutProduct")
            .WithOpenApi();

            group.MapDelete("/{id}", DeleteProduct)
            .WithName("DeleteProduct")
            .WithOpenApi();

            return group;
        }

        public static async Task<Results<Ok<ProductListDto>, NotFound<string>>> GetAllProducts(IProductService productService, string? name, string? sortBy)
        {
            var products = await productService.ListRecords(name, sortBy);

            if (products.TotalCount == 0) return TypedResults.NotFound("No products found.");

            return TypedResults.Ok(products);
        }


        public static async Task<Results<Ok<ProductReadDto>, NotFound>> GetProduct(IProductService productService, long id)
        {
            var product = await productService.GetById(id);

            if (product is null) return TypedResults.NotFound();

            return TypedResults.Ok(product);
        }

        // create todo
        public static async Task<Results<Created, BadRequest<string>, StatusCodeHttpResult>> RegisterProduct(IProductService productService, ProductCreateDto product)
        {
            try
            {
                await productService.Register(product);
                return TypedResults.Created();
            }
            catch (Exception ex)
            {
                return ex is ValidationException ?
                    TypedResults.BadRequest(ex.Message) :
                    TypedResults.StatusCode(500);
            }
        }

        // update todo
        public static async Task<Results<NoContent, BadRequest<string>, StatusCodeHttpResult>> UpdateProduct(IProductService productService, ProductUpdateDto product)
        {
            try
            {
                await productService.Update(product);
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                return ex is ValidationException ?
                    TypedResults.BadRequest(ex.Message) :
                    TypedResults.StatusCode(500);
            }
        }

        // delete todo
        public static async Task<Results<NoContent, NotFound<string>, StatusCodeHttpResult>> DeleteProduct(IProductService productService, long id)
        {
            try
            {
                await productService.Delete(id);
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                return ex is NotFoundException ?
                    TypedResults.NotFound(ex.Message) :
                    TypedResults.StatusCode(statusCode: 500);
            }
        }
    }
}