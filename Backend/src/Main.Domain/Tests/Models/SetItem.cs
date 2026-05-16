using BaseDomain.Models;

namespace Main.Domain.Tests.Models;

public class SetItem : GuidEntity
{
    public Guid SetId { get; set; }
    
    public Set Set { get; set; }
    
    public string Term { get; set; }
    
    public string Description { get; set; }
}