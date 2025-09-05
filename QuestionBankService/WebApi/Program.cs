using Application.Mappings;
using Application.Services;
using Application.Services.Implementations;
using Application.Services.Intefaces;
using Domain.Entities;
using Infrastructure.Contexts;
using Infrastructure.Seeds;
using Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
        
    });
});

// Add services to the container.
builder.Services.AddDbContext<DefaultContext>(ops => ops.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

builder.Services.AddScoped<IExamTypeService, ExamTypeService>();
builder.Services.AddScoped<ILevelService, LevelService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ISkillLevelService, SkillLevelService>();
builder.Services.AddScoped<IRangeService, RangeService>();
builder.Services.AddScoped<IContextService, ContextService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

using (IServiceScope serviceScope = app.Services.CreateScope())
{
    IServiceProvider serviceScopeProvider = serviceScope.ServiceProvider;
    DefaultContext dbContext = serviceScopeProvider.GetRequiredService<DefaultContext>();
    await Seed.ApplyAsync(dbContext, serviceScopeProvider);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
