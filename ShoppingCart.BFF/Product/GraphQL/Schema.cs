using GraphQL;
using GraphQL.Types;
using ShoppingCart.BFF.Product.Core;

namespace ShoppingCart.BFF.Product.GraphQL;

public class ProductSchema: Schema
{
    public ProductSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<ProductQueryObject>();
        Mutation = serviceProvider.GetRequiredService<ProductMutationObject>();
    }
}

public sealed class ProductObject : ObjectGraphType<ProductEntity>
{
    public ProductObject()
    {
        Name = nameof(Product);
        Description = "A product in the shopping cart";

        Field(m => m.Id).Description("Identifier of the product");
        Field(m => m.Name).Description("Name of the product");
        Field(m => m.Description).Description("Description of the product");
        Field(m => m.Price).Description("Price of the product");
        Field(
            name: "Reviews",
            description: "Reviews of the product",
            type: typeof(ListGraphType<ReviewObject>),
            resolve: m => m.Source.Reviews);
    }
}

public sealed class ProductInputObject : InputObjectGraphType<ProductInput>
{
    public ProductInputObject()
    {
        Name = nameof(ProductInput);
        Description = "The product input for building a product object";

        Field(m => m.Name).Description("Name of the product");
        Field(m => m.Description).Description("Description of the product");
        Field(m => m.Manufacturer).Description("Manufacturer of the product");
        Field(m => m.Price).Description("Price of the product");
    }
}

public sealed class ReviewObject : ObjectGraphType<ReviewEntity>
{
    public ReviewObject()
    {
        Name = nameof(ReviewEntity);
        Description = "A review of the product";
        Field(r => r.Reviewer).Description("Name of the reviewer");
        Field(r => r.Content).Description("Description from the reviewer");
        Field(r => r.Stars).Description("Star rating out of five");
    }
}

public sealed class ReviewInputObject : InputObjectGraphType<ReviewEntity>
{
    public ReviewInputObject()
    {
        Name = "ReviewInput";
        Description = "A review of the product";
        Field(r => r.Reviewer).Description("Name of the reviewer");
        Field(r => r.Content).Description("Content from the reviewer");
        Field(r => r.Stars).Description("Star rating out of five");
    }
}

public class ProductQueryObject: ObjectGraphType<object>
{
    public ProductQueryObject(IProductRepository repository)
    {
        Name = "Queries";
        Description = "The base query for all the entities in our object graph.";
        
        FieldAsync<ListGraphType<ProductObject>>(
            name: "products",
            description: "Gets all items from the shopping cart.",
            resolve: async _ => await repository.GetAllAsync());
        
        FieldAsync<ListGraphType<ReviewObject>>(
            name: "reviews",
            description: "Gets all reviews from the products in the shopping cart.",
            resolve: async _ =>
            {
                var products = await repository.GetAllAsync();
                return products.SelectMany(p => p.Reviews);
            });
        
        FieldAsync<ProductObject, ProductEntity>(
            name: "getProductById",
            description: "Gets a product by its unique identifier.",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>>
                {
                    Name = "id",
                    Description = "The unique GUID of the product."
                }),
            resolve: async context => await repository.GetByIdAsync(context.GetArgument("id", Guid.Empty)));
    }
}

public class ProductMutationObject : ObjectGraphType<object>
{
    public ProductMutationObject(IProductRepository repository)
    {
        Name = "Mutations";
        Description = "The base mutation for all the entities in our object graph.";
        
        FieldAsync<ProductObject, ProductEntity>(
            name: "addProduct",
            description: "Add product to the shopping cart.",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<ProductInputObject>>
                {
                    Name = "product",
                    Description = "The product to be added to the shopping cart."
                }),
            resolve: async context =>
            {
                var productInput = context.GetArgument<ProductInput>("product");
                return await repository.AddProductAsync(productInput);
            });

        FieldAsync<ProductObject, ProductEntity>(
            name: "addReview",
            description: "Add review to a product.",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>>
                {
                    Name = "id",
                    Description = "The unique GUID of the product."
                },
                new QueryArgument<NonNullGraphType<ReviewInputObject>>
                {
                    Name = "review",
                    Description = "Review for the product."
                }),
            resolve: async context =>
            {
                var id = context.GetArgument<Guid>("id");
                var review = context.GetArgument<ReviewEntity>("review");
                return await repository.AddReviewForProductAsync(id, review);
            });
        
        FieldAsync<ProductObject, object>(
            name: "deleteProductById",
            description: "Remove the product from the shopping cart.",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>>
                {
                    Name = "id",
                    Description = "The unique GUID of the product."
                }),
            resolve: async context =>
            {
                var id = context.GetArgument<Guid>("id");
                var result = await repository.DeleteProductById(id);
                return result.Id;
            });
    }
}