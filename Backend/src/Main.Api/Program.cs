using System.Text;
using BaseInfrastructure.DbContext;
using BaseInfrastructure.Factories;
using Main.Api.Middleware;
using Main.Application.Identity.Models;
using Main.Application.Identity.Services;
using Main.Application.Tests;
using Main.Application.Users.Handlers;
using Main.Domain.Tests.Models;
using Main.Domain.Users.Models;
using Main.Infrastructure.Adapters;
using Main.Infrastructure.DbContext;
using Main.Infrastructure.Repositories;
using Main.Infrastructure.Repositories.Tests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.Configure<ExternalAuthOptions>(builder.Configuration.GetSection("ExternalAuth"));

#region Authentication

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration
            .GetSection("Jwt")
            .Get<JwtOptions>();
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.Key))
        };
    });

#endregion

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

builder.Services.AddScoped<IRepositoryFactory<Set, Guid>, RepositoryFactory<Set, Guid>>();
builder.Services.AddScoped<ISetRepository, SetRepository>();

builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();

#endregion

#region Services (Main logic)

builder.Services.AddScoped<IExternalIdentityValidator, OidcExternalIdentityValidator>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<AddUserHandler>();
builder.Services.AddScoped<DeleteUserHandler>();
builder.Services.AddScoped<UpdateUserHandler>();
builder.Services.AddScoped<GetUserHandler>();

builder.Services.AddScoped<ISetService, SetService>();

#endregion

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MainDataContext>();
    db.Database.Migrate();
}

#region Middleware

app.UseMiddleware<ExceptionMiddleware>();

#endregion

#region Swagger

app.UseSwagger();
app.UseSwaggerUI();

#endregion

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();