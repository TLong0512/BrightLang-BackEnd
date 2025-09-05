using Application.Abstraction;
using Application.Abstraction.Repositories;
using Application.Abstraction.Services;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using WebApi.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProcessRepository, ProcessRepository>();
builder.Services.AddScoped<IRoadmapElementRepository, RoadmapElementRepository>();
builder.Services.AddScoped<IRoadmapRepository, RoadmapRepository>();
builder.Services.AddScoped<IUserRoadmapRepository, UserRoadmapRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddHttpClient<IQuestionbankService, QuestionbankService>((serviceProvider, client) =>
{
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    var url = config["ServiceUrls:QuestionBank"]
              ?? throw new InvalidOperationException("Question bank service url not found.");
    client.BaseAddress = new Uri(url);
});

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAngular");

using (IServiceScope serviceScope = app.Services.CreateScope())
{
    IServiceProvider serviceScopeProvider = serviceScope.ServiceProvider;

    // health check
    for (int i = 0; ; i++)
    {
        IQuestionbankService questionbankService = serviceScopeProvider.GetRequiredService<IQuestionbankService>();
        try
        {
            await questionbankService.GetAllLevelsAsync();
            break; // on success => healthy => break out of for loop and seed.
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Attempt {i + 1}: {ex.Message}");
            if (i >= 25) throw new InvalidOperationException("QuestionBank service is not available. Startup aborted.");
        }
        await Task.Delay(TimeSpan.FromSeconds(5));
    }
    // break wil go to here

    // seed
    AppDbContext dbContext = serviceScopeProvider.GetRequiredService<AppDbContext>();
    await Seed.ApplyAsync(dbContext, serviceScopeProvider);
}


//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
