using BaseInfrastructure.Factories;
using Main.Domain.Tutors.Models;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Repositories.Tutors;

public class FavoriteTutorRepository : IFavoriteTutorRepository
{
    private readonly IRepositoryFactory<FavoriteTutor, Guid> _repositoryFactory;

    public FavoriteTutorRepository(IRepositoryFactory<FavoriteTutor, Guid> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }
    
    public async Task UpdateFavoriteTutor(Guid userId, Guid tutorId, bool isFavorite, CancellationToken cancellationToken)
    {
                using var repository = await _repositoryFactory.CreateRepositoryAsync();
                
                var exist = await repository.Query.FirstOrDefaultAsync(e => e.TutorId == tutorId && e.UserId == userId, cancellationToken: cancellationToken);

                if (exist != null && !isFavorite)
                {
                    repository.Delete(exist);
                }
                else if (exist == null && isFavorite)
                {
                    repository.Insert(new FavoriteTutor
                    {
                        TutorId = tutorId,
                        UserId = userId
                    });
                }
                
                await repository.CommitAsync();
    }

    public async Task<List<Guid>> GetFavoriteTutorsId(Guid userId, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        return await repository.Query
            .Where(e => e.UserId == userId)
            .Select(e => e.TutorId)
            .ToListAsync(cancellationToken);
    }
}