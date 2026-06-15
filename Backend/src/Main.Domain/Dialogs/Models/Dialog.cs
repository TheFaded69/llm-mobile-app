using BaseDomain.Models;
using Main.Domain.Users.Models;

namespace Main.Domain.Dialogs.Models;

public class Dialog : GuidEntity
{
    public List<Message> Messages { get; set; } = [];
    
    public Guid UserId { get; set; }
    
    public Guid TutorId { get; set; }
}