using BaseInfrastructure.DbContext;
using Main.Domain.Identity.Models;
using Main.Domain.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.DbContext;

public class MainDataContext : DataContext 
{
    public MainDataContext(DbContextOptions<MainDataContext> options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        CreateDbUsersModel(modelBuilder);
        CreateDbIdentityModels(modelBuilder);
        CreateDbGameModels(modelBuilder);
    }
    
    private void CreateDbUsersModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        
        CreateBaseEntity<User, Guid>(modelBuilder);
        
        modelBuilder
            .Entity<User>()
            .Property(e => e.UserName);
        modelBuilder
            .Entity<User>()
            .Property(e => e.Role);
        modelBuilder
            .Entity<User>()
            .Property(e => e.UserType);
        modelBuilder
            .Entity<User>()
            .Property(e => e.Email);
        modelBuilder
            .Entity<User>()
            .HasIndex(e => e.Email)
            .IsUnique();
    }

    private void CreateDbIdentityModels(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUser>().ToTable("IdentityUsers");
        
        CreateBaseEntity<IdentityUser, Guid>(modelBuilder);
        
        modelBuilder
            .Entity<IdentityUser>()
            .Property(x => x.Email)
            .IsRequired();
        modelBuilder
            .Entity<IdentityUser>()
            .Property(x => x.PasswordHash)
            .IsRequired();
        modelBuilder
            .Entity<IdentityUser>()
            .Property(x => x.UserName)
            .IsRequired();
        modelBuilder
            .Entity<IdentityUser>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder
            .Entity<RefreshToken>()
            .ToTable("RefreshTokens");
        
        CreateBaseEntity<RefreshToken, Guid>(modelBuilder);
        
        modelBuilder
            .Entity<RefreshToken>()
            .Property(x => x.TokenHash)
            .IsRequired();
        modelBuilder
            .Entity<RefreshToken>()
            .HasIndex(x => x.TokenHash)
            .IsUnique();
        modelBuilder
            .Entity<RefreshToken>()
            .HasIndex(x => x.IdentityUserId);

        modelBuilder.Entity<PasswordResetToken>().ToTable("PasswordResetTokens");
        
        CreateBaseEntity<PasswordResetToken, Guid>(modelBuilder);
        
        modelBuilder
            .Entity<PasswordResetToken>()
            .Property(x => x.TokenHash)
            .IsRequired();
        modelBuilder
            .Entity<PasswordResetToken>()
            .HasIndex(x => x.TokenHash)
            .IsUnique();

        modelBuilder.Entity<ExternalLogin>().ToTable("ExternalLogins");
        
        CreateBaseEntity<ExternalLogin, Guid>(modelBuilder);
        
        modelBuilder
            .Entity<ExternalLogin>()
            .Property(x => x.Provider)
            .IsRequired();
        modelBuilder
            .Entity<ExternalLogin>()
            .Property(x => x.ProviderUserId)
            .IsRequired();
        modelBuilder.Entity<ExternalLogin>()
            .HasIndex(x => new { x.Provider, x.ProviderUserId })
            .IsUnique();
    }

    private void CreateDbGameModels(ModelBuilder modelBuilder)
    {
        
    }
}