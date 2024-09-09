using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
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

        public static async Task<IResult> RegisterProduct(IProductService productService, ProductCreateDto product)
        {
            try
            {
                await productService.Register(product);
                return TypedResults.Created();
            }
            catch (Exception ex)
            {
                return HandleException(ex, product.Name);
            }
        }

        public static async Task<IResult> UpdateProduct(IProductService productService, ProductUpdateDto product)
        {
            try
            {
                await productService.Update(product);
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        public static async Task<IResult> DeleteProduct(IProductService productService, long id)
        {
            try
            {
                await productService.Delete(id);
                return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        public static IResult HandleException(Exception ex, string entityName = "")
        {
            if (ex is DbUpdateException dbUpdateEx && dbUpdateEx.InnerException != null)
            {
                if (dbUpdateEx.InnerException.Message.Contains("unique constraint"))
                {
                    return TypedResults.Conflict($"An entity with the same name '{entityName}' already exists!");
                }
            }
            else if (ex is ValidationException validationEx)
            {
                return TypedResults.BadRequest(validationEx.Message);
            }
            else if (ex is NotFoundException validationNotFoundEx)
            {
                return TypedResults.NotFound(validationNotFoundEx.Message);
            }
            
            return TypedResults.StatusCode(500);
        }
    }
}