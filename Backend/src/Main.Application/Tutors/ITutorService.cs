using Main.Contract.Tutors.V1.Requests;
using Main.Contract.Tutors.V1.Responses;

namespace Main.Application.Tutors;

public interface ITutorService
{
    Task<GetTutorsResponse> GetTutors(Guid userId, CancellationToken cancellationToken);
    
    Task<GetTutorResponse> GetTutor(Guid id, CancellationToken cancellationToken);
    
    Task<GetTutorsResponse> GetMyTutors(Guid userId, CancellationToken cancellationToken);
    
    Task<GetTutorsResponse> GetFavoriteTutors(Guid userId, CancellationToken cancellationToken);
    
    Task UpdateTutor(Guid id, UpdateTutorRequest request, CancellationToken cancellationToken);
    
    Task<CreateTutorResponse> CreateTutor(Guid userId, CreateTutorRequest request, CancellationToken cancellationToken);
    
    Task DeleteTutor(Guid id, CancellationToken cancellationToken);
    
    Task SetTutorFavorite(Guid userId, Guid tutorId, bool isFavorite, CancellationToken cancellationToken);
}