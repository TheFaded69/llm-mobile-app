using BaseInfrastructure.Factories;
using Main.Domain.Tutors.Models;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Repositories.Tutors;

public class TutorRepository : ITutorRepository
{
    private readonly IRepositoryFactory<Tutor, Guid> _repositoryFactory;

    public TutorRepository(IRepositoryFactory<Tutor, Guid> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }
    
    public async Task<List<Tutor>> GetTutors(CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        return await repository.Query
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<Tutor>> GetFavoriteTutors(List<Guid> tutorsId, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        return await repository.Query
            .Where(t => tutorsId.Contains(t.Id))
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<Tutor>> GetMyTutors(Guid userId, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();

        return await repository.Query
            .Where(t => t.UserId == userId)
            .ToListAsync(cancellationToken: cancellationToken);
    }
    
    public async Task<Tutor?> GetTutorWithDetails(Guid id, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        return await repository.Query
            .Include(t => t.TargetWords)
            .Include(t => t.Stories)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task UpdateTutor(Tutor tutor, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
                
        repository.Update(tutor);
        
        await repository.CommitAsync();
    }

    public async Task<Guid> CreateTutor(Guid userId, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();

        var tutor = new Tutor()
        {
            UserId = userId,
        };
        
        repository.Insert(tutor);
        
        await repository.CommitAsync();
        
        return tutor.Id;
    }

    public async Task DeleteTutor(Guid id, CancellationToken cancellationToken)
    {
        using var repository = await _repositoryFactory.CreateRepositoryAsync();
        
        var tutor = await repository.GetAsync(id, cancellationToken) ?? throw new Exception($"Такого tutor [{id}] не существует");
        
        repository.Delete(tutor);
        
        await repository.CommitAsync();
    }
}