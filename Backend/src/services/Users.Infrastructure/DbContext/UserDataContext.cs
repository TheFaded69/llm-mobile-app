using BaseInfrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Models;

namespace Users.Infrastructure.DbContext;

public class UserDataContext : DataContext 
{
    public UserDataContext(DbContextOptions<UserDataContext> options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        CreateDbUsersModel(modelBuilder);
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
}