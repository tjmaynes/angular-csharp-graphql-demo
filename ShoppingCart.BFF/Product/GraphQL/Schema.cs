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

public sealed class ReviewObject : ObjectGraphType<Review>
{
    public ReviewObject()
    {
        Name = nameof(Review);
        Description = "A review of the product";
        Field(r => r.Reviewer).Description("Name of the reviewer");
        Field(r => r.Content).Description("Description from the reviewer");
        Field(r => r.Stars).Description("Star rating out of five");
    }
}

public sealed class ReviewInputObject : InputObjectGraphType<Review>
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
                new QueryArgument<NonNullGraphType<StringGraphType>>
                {
                    Name = "name",
                    Description = "Name of the product."
                },
                new QueryArgument<NonNullGraphType<StringGraphType>>
                {
                    Name = "description",
                    Description = "Description of the product."
                },
                new QueryArgument<NonNullGraphType<DecimalGraphType>>
                {
                    Name = "price",
                    Description = "Price of the product."
                }),
            resolve: async context =>
            {
                var name = context.GetArgument<string>("name");
                var description = context.GetArgument<string>("description");
                var price = context.GetArgument<Double>("price");
                return await repository.AddProductAsync(name, description, price);
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
                var review = context.GetArgument<Review>("review");
                return await repository.AddReviewForProductAsync(id, review);
            });
    }
}