namespace BaseDomain.Models;

public class GuidEntity : Entity<Guid>
{
    protected override bool IsEmpty(Guid id) => Id == Guid.Empty;
}