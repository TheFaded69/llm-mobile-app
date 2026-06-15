namespace Main.Infrastructure.Repositories.Tests;

public interface IFavoriteSetRepository
{
    Task UpdateFavoriteSet(Guid userId, Guid setId, bool isFavorite, CancellationToken cancellationToken);
    
    Task<List<Guid>> GetFavoriteSetsId(Guid userId, CancellationToken cancellationToken);
}