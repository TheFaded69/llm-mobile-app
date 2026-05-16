using BaseDomain.Models;

namespace Main.Domain.Tests.Models.Questions;

public class TestQuestionSelectionAnswer : GuidEntity
{
    public Guid TestQuestionSelectionId { get; set; }
    
    public TestQuestionSelection TestQuestionSelection { get; set; }
    
    public string Answer { get; set; }
}