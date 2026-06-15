using BaseDomain.Models;
using Main.Domain.Tutors.Enums;
using Main.Domain.Users.Models;

namespace Main.Domain.Tutors.Models;

public class Tutor : GuidEntity
{
    public Guid UserId { get; set; }
    
    public string TutorRole { get; set; }
    
    public string Description { get; set; }
    
    public string Personality { get; set; }
    
    public string Name { get; set; }
    
    public bool IsPublic { get; set; }
    
    public TutorDifficulty Difficulty { get; set; }
    
    public List<StoryLine> Stories { get; set; }
    
    public List<TargetWord> TargetWords { get; set; }
}