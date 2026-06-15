using Main.Domain.Tutors.Models;

namespace Main.Infrastructure.Repositories.Tutors;

public interface ITutorRepository
{
    Task<List<Tutor>> GetTutors(CancellationToken cancellationToken);
    
    Task<List<Tutor>> GetFavoriteTutors(List<Guid> tutorsId, CancellationToken cancellationToken);
    
    Task<List<Tutor>> GetMyTutors(Guid userId, CancellationToken cancellationToken);
    
    Task<Tutor?> GetTutorWithDetails(Guid id, CancellationToken cancellationToken);
    
    Task UpdateTutor(Tutor tutor, CancellationToken cancellationToken);
    
    Task<Guid> CreateTutor(Guid userId, CancellationToken cancellationToken);
    
    Task DeleteTutor(Guid id, CancellationToken cancellationToken);
}