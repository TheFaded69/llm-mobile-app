using BaseInfrastructure.DbContext;
using BaseInfrastructure.Factories;
using Main.Application.Identity.Models;
using Main.Application.Identity.Services;
using Main.Application.Users.Handlers;
using Main.Domain.Users.Models;
using Main.Infrastructure.Adapters;
using Main.Infrastructure.DbContext;
using Main.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Main API",
        Version = "v1"
    });
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

#region DataContext

builder.Services.AddDbContextFactory<MainDataContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("main_connection"));
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddScoped<IDbContextFactory<DataContext>>(sp =>
{
    var inner = sp.GetRequiredService<IDbContextFactory<MainDataContext>>();
    return new DataContextFactoryAdapter(inner);
});

#endregion

#region Repositories

builder.Services.AddScoped<IRepositoryFactory<User, Guid>, RepositoryFactory<User, Guid>>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();

#endregion

#region Services (Main logic)

builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<AddUserHandler>();
builder.Services.AddScoped<DeleteUserHandler>();
builder.Services.AddScoped<UpdateUserHandler>();
builder.Services.AddScoped<GetUserHandler>();

#endregion

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();