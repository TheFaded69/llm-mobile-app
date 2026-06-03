using BaseDomain.Models;

namespace Main.Domain.Tutors.Models;

public class FavoriteTutor : GuidEntity
{
    public Guid UserId { get; set; }
    
    public Guid TutorId { get; set; }
}