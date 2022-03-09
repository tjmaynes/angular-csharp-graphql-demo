var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQL();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());

app
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapGraphQL();
    });

app.Run();
