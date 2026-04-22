using Microsoft.OpenApi;
using GameModes.Application.Services;
using GameModes.Infrastructure.Repositories;

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

builder.Services.AddSingleton<IGameModesDataRepository, InMemoryGameModesDataRepository>();
builder.Services.AddScoped<GameModesService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
