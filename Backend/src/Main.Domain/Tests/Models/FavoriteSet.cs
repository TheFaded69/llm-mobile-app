using BaseDomain.Models;

namespace Main.Domain.Tests.Models;

public class FavoriteSet : GuidEntity
{
    public Guid UserId { get; set; }
    
    public Guid SetId { get; set; }
}