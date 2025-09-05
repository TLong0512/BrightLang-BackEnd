using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);

// Load Ocelot config
builder.Configuration
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Register Ocelot + SwaggerForOcelot
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

// ✅ Register Swashbuckle services (required by SwaggerForOcelot)
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

// Expose aggregated Swagger UI
//if (app.Environment.IsDevelopment())
{
    app.UseSwaggerForOcelotUI();
}

// Run Ocelot middleware
await app.UseOcelot();

app.Run();
