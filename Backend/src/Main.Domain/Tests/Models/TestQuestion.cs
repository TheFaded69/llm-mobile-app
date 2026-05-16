using BaseDomain.Models;
using Main.Domain.Tests.Enums;

namespace Main.Domain.Tests.Models;

public class TestQuestion : GuidEntity
{
   public string Definition { get; set; }
    
    public string ExplainTerm { get; set; }
    
    public string ExplainText { get; set; }
}