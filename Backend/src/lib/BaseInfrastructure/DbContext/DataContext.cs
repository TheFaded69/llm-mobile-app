using BaseDomain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BaseInfrastructure.DbContext;

public class DataContext : Microsoft.EntityFrameworkCore.DbContext, IDataContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
        //Database.EnsureCreated();
    }

    private IDbContextTransaction _transaction;
    
    public async Task BeginTransactionAsync() => _transaction = await Database.BeginTransactionAsync();

    private void DetachAllEntities()
    {
        var changedEntriesCopy = ChangeTracker.Entries()
            .Where(e => e.State != EntityState.Detached)
            .ToArray();

        foreach (var entry in changedEntriesCopy)
        {
            entry.State = EntityState.Detached;
        }
    }
    
    public async Task CommitAsync()
    {
        try
        {
            await SaveChangesAsync();
            if (_transaction != null) await _transaction.CommitAsync();
        }
        finally
        {
            _transaction?.Dispose();
            DetachAllEntities();
        }
    }
    
    public void Commit()
    {
        try
        {
            SaveChanges();
            _transaction?.Commit();
        }
        finally
        {
            _transaction?.Dispose();
            DetachAllEntities();
        }
    }
    
    public async Task RollbackAsync()
    {
        try
        {
            await _transaction.RollbackAsync();
            _transaction.Dispose();
            DetachAllEntities();
        }
        catch
        {
            // absorb all exceptions
        }
    }
    
    /// <summary>
    /// Создание основных свойств модели
    /// </summary>
    /// <param name="modelBuilder">билдер модели</param>
    /// <typeparam name="TEntityType">тип иодели БД</typeparam>
    /// <typeparam name="TKeyType">тип ключа модели БД</typeparam>
    protected void CreateBaseEntity<TEntityType, TKeyType>(ModelBuilder modelBuilder) where TEntityType : Entity<TKeyType>
    {
        modelBuilder
            .Entity<TEntityType>()
            .HasKey(p => p.Id);
        
        modelBuilder
            .Entity<TEntityType>()
            .Property(e => e.CreateTime)
            .HasDefaultValueSql("NOW()");
        
        modelBuilder
            .Entity<TEntityType>()
            .Property(e => e.Deleted);
    }
}