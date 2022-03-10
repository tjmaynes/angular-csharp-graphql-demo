using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.Types;
using ShoppingCart.BFF.Product.Core;
using ShoppingCart.BFF.Product.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o =>
{
    o.AddPolicy("MyCors", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .WithMethods(HttpMethod.Post.ToString());
    });
});

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
                Stars = Rating.AMAZING
            },
            new Review
            {
                Id = Guid.NewGuid(),
                Reviewer = "Rick Sanchez",
                Content = "I've had better...",
                Stars = Rating.GOOD
            }
        }
    },
    new ProductEntity
    {
        Id = Guid.NewGuid(),
        Name = "Graham Crackers",
        Description = "Cinnamon, milk and ginger",
        Price = 2.99,
        Reviews = new[]
        {
            new Review
            {
                Id = Guid.NewGuid(),
                Reviewer = "Jerry Smith",
                Content = "I love this brand! So good in dunked in milk.",
                Stars = Rating.AMAZING
            },
            new Review
            {
                Id = Guid.NewGuid(),
                Reviewer = "Morty Smith",
                Content = "Totally agree, Dad! So good dunked in milk.",
                Stars = Rating.AMAZING
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

app.UseCors("MyCors");

app.UseDeveloperExceptionPage();

app.UseGraphQLAltair();
app.UseGraphQL<ISchema>();

app.Run();
