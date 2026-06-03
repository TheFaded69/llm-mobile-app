using BaseDomain.Models;
using Main.Domain.Tests.Enums;
using Main.Domain.Users.Models;

namespace Main.Domain.Tests.Models;

public class Set : GuidEntity
{
    public Guid UserId { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public bool IsPublic { get; set; }
    
    public SetStatus SetStatus { get; set; }
    
    public TestDifficult TestDifficult { get; set; }
    
    public int Duration { get; set; }
    
    public List<SetItem> SetItems { get; set; }
}