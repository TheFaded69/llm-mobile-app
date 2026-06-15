using Main.Domain.Tutors.Enums;

namespace Main.Contract.Tutors.V1.DTO;

public class TutorMetaDTO
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public TutorDifficulty Difficulty { get; set; }
    
    public bool IsFavorite { get; set; }
}