using Main.Contract.Tutors.V1.DTO;
using Main.Contract.Tutors.V1.Requests;
using Main.Contract.Tutors.V1.Responses;
using Main.Domain.Tutors.Enums;
using Main.Domain.Tutors.Models;
using Main.Infrastructure.Repositories.Tutors;

namespace Main.Application.Tutors;

public class TutorService : ITutorService
{
    private readonly ITutorRepository _tutorRepository;
    private readonly IFavoriteTutorRepository _favoriteTutorRepository;

    public TutorService(
        ITutorRepository  tutorRepository,
        IFavoriteTutorRepository favoriteTutorRepository
        )
    {
        _tutorRepository = tutorRepository;
        _favoriteTutorRepository = favoriteTutorRepository;
    }
    
    public async Task<GetTutorsResponse> GetTutors(Guid userId, CancellationToken cancellationToken)
    {
        var tutors = await _tutorRepository.GetTutors(cancellationToken);
        
        var favoriteTutorsId = await _favoriteTutorRepository.GetFavoriteTutorsId(userId, cancellationToken);
        
        var favoriteTutors = await _tutorRepository.GetFavoriteTutors(favoriteTutorsId, cancellationToken);

        var favoriteTutorIds = favoriteTutors
            .Select(tutor => tutor.Id)
            .ToHashSet();

        var tutorsMeta = favoriteTutors
            .Select(tutor => new TutorMetaDTO
            {
                Id = tutor.Id,
                Name = tutor.Name,
                Difficulty = tutor.Difficulty,
                IsFavorite = true
            })
            .Concat(
                tutors
                    .Where(tutor => !favoriteTutorIds.Contains(tutor.Id))
                    .Select(tutor => new TutorMetaDTO
                    {
                        Id = tutor.Id,
                        Name = tutor.Name,
                        Difficulty = tutor.Difficulty,
                        IsFavorite = false
                    })
            )
            .ToList();

        return new GetTutorsResponse
        {
            TutorsMeta = tutorsMeta
        };
    }

    public async Task<GetTutorResponse> GetTutor(Guid id, CancellationToken cancellationToken)
    {
        var tutor = await _tutorRepository.GetTutorWithDetails(id, cancellationToken);

        return new GetTutorResponse()
        {
            Tutor = new TutorDTO
            {
                Id = tutor.Id,
                Name = tutor.Name,
                TutorRole = tutor.TutorRole,
                Description = tutor.Description,
                Personality = tutor.Personality,
                IsPublic = tutor.IsPublic,
                Difficulty = tutor.Difficulty,
                Stories = tutor.Stories.Select(s => new StoryLineDTO
                {
                    StoryLineId = s.Id,
                    Story = s.Story
                }).ToList(),
                
                TargetWords = tutor.TargetWords.Select(w => new TargetWordDTO
                {
                    WordId = w.Id,
                    Word = w.Word
                }).ToList()
            }
        };
    }

    public async Task<GetTutorsResponse> GetMyTutors(Guid userId, CancellationToken cancellationToken)
    {
        var favoriteTutors = await _tutorRepository.GetMyTutors(userId, cancellationToken);
        
        var tutorsMeta = favoriteTutors
            .Select(tutor => new TutorMetaDTO
            {
                Id = tutor.Id,
                Name = tutor.Name,
                Difficulty = tutor.Difficulty,
                IsFavorite = true
            })
            .ToList();

        return new GetTutorsResponse
        {
            TutorsMeta = tutorsMeta
        };
    }

    public async Task<GetTutorsResponse> GetFavoriteTutors(Guid userId, CancellationToken cancellationToken)
    {
        var favoriteTutorsId = await _favoriteTutorRepository.GetFavoriteTutorsId(userId, cancellationToken);
        var favoriteTutors = await _tutorRepository.GetFavoriteTutors(favoriteTutorsId, cancellationToken);
        
        var tutorsMeta = favoriteTutors
            .Select(tutor => new TutorMetaDTO
            {
                Id = tutor.Id,
                Name = tutor.Name,
                Difficulty = tutor.Difficulty,
                IsFavorite = true
            })
            .ToList();

        return new GetTutorsResponse
        {
            TutorsMeta = tutorsMeta
        };
    }

    public async Task UpdateTutor(Guid id, UpdateTutorRequest request, CancellationToken cancellationToken)
    {
        var tutor = await _tutorRepository.GetTutorWithDetails(id, cancellationToken);
        
        var dto = request.Tutor;
        
        tutor.Name = dto.Name;
        tutor.TutorRole = dto.TutorRole;
        tutor.Description = dto.Description;
        tutor.Personality = dto.Personality;
        tutor.IsPublic = dto.IsPublic;
        tutor.Difficulty = dto.Difficulty;
        
        SyncStories(tutor, dto.Stories);
        SyncTargetWords(tutor, dto.TargetWords);

        await _tutorRepository.UpdateTutor(tutor, cancellationToken);
    }
    
    private void SyncStories(Tutor tutor, List<StoryLineDTO> incoming)
    {
        incoming ??= [];

        // 1. удалить отсутствующие
        var incomingIds = incoming
            .Where(x => x.StoryLineId != Guid.Empty)
            .Select(x => x.StoryLineId)
            .ToHashSet();

        var toRemove = tutor.Stories
            .Where(x => x.Id != Guid.Empty && !incomingIds.Contains(x.Id))
            .ToList();

        foreach (var item in toRemove)
        {
            tutor.Stories.Remove(item);
        }

        // 2. add/update
        foreach (var dto in incoming)
        {
            var existing = tutor.Stories.FirstOrDefault(x => x.Id == dto.StoryLineId);

            if (existing is null)
            {
                tutor.Stories.Add(new StoryLine
                {
                    TutorId = tutor.Id,
                    Story = dto.Story,
                });
            }
            else
            {
                existing.Story = dto.Story;
            }
        }
    }
    
    private void SyncTargetWords(Tutor tutor, List<TargetWordDTO> incoming)
    {
        incoming ??= [];

        var incomingIds = incoming
            .Where(x => x.WordId != Guid.Empty)
            .Select(x => x.WordId)
            .ToHashSet();

        var toRemove = tutor.TargetWords
            .Where(x => x.Id != Guid.Empty && !incomingIds.Contains(x.Id))
            .ToList();

        foreach (var item in toRemove)
        {
            tutor.TargetWords.Remove(item);
        }

        foreach (var dto in incoming)
        {
            var existing = tutor.TargetWords.FirstOrDefault(x => x.Id == dto.WordId);

            if (existing is null)
            {
                tutor.TargetWords.Add(new TargetWord
                {
                    Word = dto.Word,
                    TutorId = tutor.Id,
                });
            }
            else
            {
                existing.Word = dto.Word;
            }
        }
    }

    public async Task<CreateTutorResponse> CreateTutor(Guid userId, CreateTutorRequest request, CancellationToken cancellationToken)
    {
        return new CreateTutorResponse()
        {
            TutorId = await _tutorRepository.CreateTutor(userId, cancellationToken)
        };
    }

    public async Task DeleteTutor(Guid id, CancellationToken cancellationToken)
    {
        await _tutorRepository.DeleteTutor(id, cancellationToken);
    }

    public async Task SetTutorFavorite(Guid userId, Guid tutorId, bool isFavorite, CancellationToken cancellationToken)
    {
        await _favoriteTutorRepository.UpdateFavoriteTutor(userId, tutorId, isFavorite, cancellationToken);
    }
}