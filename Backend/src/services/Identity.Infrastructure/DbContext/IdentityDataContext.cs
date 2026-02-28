using BaseInfrastructure.DbContext;
using Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.DbContext;

public class IdentityDataContext : DataContext
{
    public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUser>().ToTable("IdentityUsers");
        CreateBaseEntity<IdentityUser, Guid>(modelBuilder);
        modelBuilder.Entity<IdentityUser>().Property(x => x.Email).IsRequired();
        modelBuilder.Entity<IdentityUser>().Property(x => x.PasswordHash).IsRequired();
        modelBuilder.Entity<IdentityUser>().Property(x => x.UserName).IsRequired();
        modelBuilder.Entity<IdentityUser>().HasIndex(x => x.Email).IsUnique();

        modelBuilder.Entity<RefreshToken>().ToTable("RefreshTokens");
        CreateBaseEntity<RefreshToken, Guid>(modelBuilder);
        modelBuilder.Entity<RefreshToken>().Property(x => x.TokenHash).IsRequired();
        modelBuilder.Entity<RefreshToken>().HasIndex(x => x.TokenHash).IsUnique();
        modelBuilder.Entity<RefreshToken>().HasIndex(x => x.IdentityUserId);

        modelBuilder.Entity<PasswordResetToken>().ToTable("PasswordResetTokens");
        CreateBaseEntity<PasswordResetToken, Guid>(modelBuilder);
        modelBuilder.Entity<PasswordResetToken>().Property(x => x.TokenHash).IsRequired();
        modelBuilder.Entity<PasswordResetToken>().HasIndex(x => x.TokenHash).IsUnique();

        modelBuilder.Entity<ExternalLogin>().ToTable("ExternalLogins");
        CreateBaseEntity<ExternalLogin, Guid>(modelBuilder);
        modelBuilder.Entity<ExternalLogin>().Property(x => x.Provider).IsRequired();
        modelBuilder.Entity<ExternalLogin>().Property(x => x.ProviderUserId).IsRequired();
        modelBuilder.Entity<ExternalLogin>()
            .HasIndex(x => new { x.Provider, x.ProviderUserId })
            .IsUnique();
    }
}
