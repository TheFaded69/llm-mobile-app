using BaseInfrastructure.DbContext;
using GameModes.Application.Services;
using GameModes.Infrastructure.Adapters;
using GameModes.Infrastructure.DbContext;
using GameModes.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GameModes API",
        Version = "v1"
    });
});

builder.Services.AddDbContextFactory<GameModesDataContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("gamemode_connection"));
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddScoped<IDbContextFactory<DataContext>>(sp =>
{
    var inner = sp.GetRequiredService<IDbContextFactory<GameModesDataContext>>();
    return new DataContextFactoryAdapter(inner);
});

builder.Services.AddScoped<IGameModesDataRepository, GameModesDataRepository>();
builder.Services.AddScoped<GameModesService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
