using BaseDomain.Models;
using Main.Domain.Tests.Enums;
using Main.Domain.Users.Models;

namespace Main.Domain.Tests.Models;

public class Session : GuidEntity
{
    public Guid UserId { get; set; }
    
    public User User { get; set; }
    
    public Guid SetId { get; set; }
    
    public Set Set { get; set; }
    
    public Guid? DeviceId { get; set; }
    
    public TestMode TestMode { get; set; }
    
    public SessionStatus  SessionStatus { get; set; }
    
    public List<SessionItem> SessionItems { get; set; }
}