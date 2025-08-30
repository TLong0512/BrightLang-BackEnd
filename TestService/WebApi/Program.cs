using Application.Services.Implementations;
using Application.Services.Interfaces;
using Infrastructure.Contexts;
using Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<ITestQuestionService, TesQuestionService>();
builder.Services.AddHttpClient();

builder.Services.AddHttpOnlyOrDefaultJwt(new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,

    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
    ValidAudience = builder.Configuration["JwtSettings:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!))
});
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddDbContext<DefaultContext>(ops => ops.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
