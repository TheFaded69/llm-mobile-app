using BaseDomain.Models;

namespace Main.Domain.Tutors.Models;

public class TargetWord : GuidEntity
{
    public Guid TutorId { get; set; }
    
    public string Word { get; set; }
}