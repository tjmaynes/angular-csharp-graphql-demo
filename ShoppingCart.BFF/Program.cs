using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.Types;
using ShoppingCart.BFF.Helpers;
using ShoppingCart.BFF.Product.Core;
using ShoppingCart.BFF.Product.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddCors(o =>
{
    o.AddPolicy("MyCors", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .WithMethods(HttpMethod.Post.ToString());
    });
});

builder.Services.AddSingleton<ISeeder, Seeder>();

builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>(
    services =>
    {
        var seeder = services.GetService<ISeeder>()!;
        return new InMemoryProductRepository(seeder.Products);
    });

builder.Services.AddSingleton<ISchema, ProductSchema>(
    services => new ProductSchema(new SelfActivatingServiceProvider(services)));

builder.Services.AddGraphQL(options => { options.EnableMetrics = true; })
    .AddSystemTextJson()
    .AddGraphTypes();

var app = builder.Build();

app.UseHealthChecks("/healthcheck");

app.UseCors("MyCors");

app.UseDeveloperExceptionPage();

app.UseGraphQLAltair();
app.UseGraphQL<ISchema>();

app.Run();

public partial class Program { }