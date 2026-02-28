using BaseInfrastructure.DbContext;
using BaseInfrastructure.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Users.Application.Handlers;
using Users.Domain.Models;
using Users.Infrastructure.Adapters;
using Users.Infrastructure.DbContext;
using Users.Infrastructure.Factories;
using Users.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Users API",
        Version = "v1"
    });
});

builder.Services.AddDbContextFactory<UserDataContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("user_connection"));
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddScoped<IDbContextFactory<DataContext>>(sp =>
{
    var inner = sp.GetRequiredService<IDbContextFactory<UserDataContext>>();
    return new DataContextFactoryAdapter(inner);
});

builder.Services.AddScoped<IRepositoryFactory<User, Guid>, RepositoryFactory<User, Guid>>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<AddUserHandler>();
builder.Services.AddScoped<DeleteUserHandler>();
builder.Services.AddScoped<UpdateUserHandler>();
builder.Services.AddScoped<GetUserHandler>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

