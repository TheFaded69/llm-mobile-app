using BaseDomain.Models;

namespace Main.Domain.Dictionary.Models;

public class UserDictionary : GuidEntity
{
    public Guid UserId { get; set; }
    
    public List<DictionaryWord> Words { get; set; }
}