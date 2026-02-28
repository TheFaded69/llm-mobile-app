namespace Models;

public class DbEntityGuid : DbEntity<Guid>
{
    protected override bool IsEmpty(Guid id) => Id == Guid.Empty;
}