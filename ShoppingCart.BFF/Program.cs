using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.Types;
using ShoppingCart.BFF.Product.Core;
using ShoppingCart.BFF.Product.GraphQL;

var builder = WebApplication.CreateBuilder(args);

var products = new[]
{
    new ProductEntity
    {
        Id = Guid.NewGuid(),
        Name = "Orange Juice",
        Description = "Florida's best!",
        Price = 5.99,
        Reviews = new[]
        {
            new Review
            {
                Id = Guid.NewGuid(),
                Reviewer = "Jerry Smith",
                Content = "The best OJ I've ever had!",
                Stars = Rating.Amazing
            },
            new Review
            {
                Id = Guid.NewGuid(),
                Reviewer = "Rick Sanchez",
                Content = "I've had better...",
                Stars = Rating.Good
            }
        }
    }
};

builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>(
    services => new InMemoryProductRepository(products));

builder.Services.AddSingleton<ISchema, ProductSchema>(
    services => new ProductSchema(new SelfActivatingServiceProvider(services)));

builder.Services.AddGraphQL(options =>
    {
        options.EnableMetrics = true;
    })
    .AddSystemTextJson()
    .AddGraphTypes();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseGraphQLAltair();
app.UseGraphQL<ISchema>();

app.Run();
