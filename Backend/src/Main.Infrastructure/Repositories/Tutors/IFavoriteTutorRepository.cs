namespace Main.Infrastructure.Repositories.Tutors;

public interface IFavoriteTutorRepository
{
    Task UpdateFavoriteTutor(Guid userId, Guid tutorId, bool isFavorite, CancellationToken cancellationToken);
    
    Task<List<Guid>> GetFavoriteTutorsId(Guid userId, CancellationToken cancellationToken);
}