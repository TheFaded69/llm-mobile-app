using BaseDomain.Models;

namespace Main.Domain.Tests.Models.Questions;

public class TestQuestionQuestionOption : GuidEntity
{
    public Guid TestQuestionQuestionId { get; set; }
    
    public TestQuestionQuestion TestQuestionQuestion  { get; set; }
    
    public string Option { get; set; }
}