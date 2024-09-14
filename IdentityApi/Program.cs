using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using IdentityApi;
using IdentityApi.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();


builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlite("Data Source=app.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.MapGet("users/me", async (ClaimsPrincipal claims, ApplicationDbContext context) =>
    {
        string userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        return await context.Users.FindAsync(userId);
    })
    .RequireAuthorization();

app.UseHttpsRedirection();

app.MapIdentityApi<User>();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
