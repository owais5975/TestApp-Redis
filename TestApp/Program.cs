using Microsoft.EntityFrameworkCore;
using TestApp.Entities;
using TestApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDBContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("cs")));
builder.Services.AddTransient<IUserRepo, UserRepo>();
builder.Services.AddTransient<ICacheService, CacheService>();
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetConnectionString("redis");
    opt.InstanceName = "Staging-free-db";
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
