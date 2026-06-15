using BaseInfrastructure.Factories;
using Main.Domain.Tests.Models;
using Main.Domain.Tutors.Models;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Repositories.Tests;

public class FavoriteSetRepository : IFavoriteSetRepository
{
    private readonly IRepositoryFactory<FavoriteSet, Guid> _repositoryFactory;

    public FavoriteSetRepository(IRepositoryFactory<FavoriteSet, Guid> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task UpdateFavoriteSet(Guid userId, Guid setId, bool isFavorite, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        var exist = await repository.Query.FirstOrDefaultAsync(favoriteSet => favoriteSet.SetId == setId 
                                                                              && favoriteSet.UserId == userId, 
            cancellationToken: cancellationToken);

        if (exist != null && !isFavorite)
        {
            repository.Delete(exist);
        }
        else if (exist == null && isFavorite)
        {
            repository.Insert(new FavoriteSet()
            {
                SetId = setId,
                UserId = userId
            });
        }
        
        await repository.CommitAsync();
    }

    public async Task<List<Guid>> GetFavoriteSetsId(Guid userId, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        return await repository.Query
            .Where(e => e.UserId == userId)
            .Select(e => e.SetId)
            .ToListAsync(cancellationToken);
    }
}