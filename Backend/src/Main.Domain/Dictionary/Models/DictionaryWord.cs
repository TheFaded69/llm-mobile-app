using BaseDomain.Models;

namespace Main.Domain.Dictionary.Models;

public class DictionaryWord : GuidEntity
{
    public Guid DictionaryId { get; set; }
    
    public string Word { get; set; }
    
    public string Translation { get; set; }
}