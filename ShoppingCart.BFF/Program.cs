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

builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>(
    services => new InMemoryProductRepository());

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
