using BaseDomain.Models;

namespace Main.Domain.Tutors.Models;

public class StoryLine : GuidEntity
{
    public Guid TutorId { get; set; }
    
    public string Story { get; set; }
}