using TramPocketBackend.Configuration;
using TramPocketBackend.Initializers;
using TramPocketBackend.Services.Gtfs;
using TramPocketBackend.State;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.Configure<GtfsOptions>(
    builder.Configuration.GetSection("Gtfs"));
builder.Services.Configure<DeparturesOptions>(
    builder.Configuration.GetSection("Departures"));
builder.Services.AddHostedService<GtfsService>();
builder.Services.AddSingleton<GtfsParserService>();
builder.Services.AddSingleton<DataDirectoryInitializer>();
builder.Services.AddSingleton<TransitState>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add NSwag/Swagger UI
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "TramPocket API";
    config.Version = "1.0.0";
    config.Description = "API for accessing tram stop and route information";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Add NSwag Swagger UI
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Initializers calls
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DataDirectoryInitializer>();
    initializer.Initialize();
}

app.Run();