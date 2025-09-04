using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add Ocelot config (with Routes + SwaggerEndpoints)
builder.Configuration
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// AddOcelot depends on this
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Ocelot services
builder.Services.AddOcelot(builder.Configuration);

// Register SwaggerForOcelot services
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();


//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use Swagger UI aggregated by Ocelot
app.UseSwaggerForOcelotUI();

// Use Ocelot middleware
await app.UseOcelot();

app.Run();
