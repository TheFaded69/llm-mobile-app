using Main.Domain.Tutors.Enums;
using Main.Domain.Tutors.Models;

namespace Main.Contract.Tutors.V1.DTO;

public class TutorDTO
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string TutorRole { get; set; }
    
    public string Description { get; set; }
    
    public string Personality { get; set; }
    
    public bool IsPublic { get; set; }
    
    public TutorDifficulty Difficulty { get; set; }
    
    public List<StoryLineDTO> Stories { get; set; }
    
    public List<TargetWordDTO> TargetWords { get; set; }
}