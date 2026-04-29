using BaseDomain.Models;
using Main.Domain.Tests.Enums;

namespace Main.Domain.Tests.Models;

public class SessionItem : GuidEntity
{
    public Guid SessionId { get; set; }
    
    public Session Session { get; set; }
    
    public Guid QuestionId { get; set; }
    
    public TestQuestion Question { get; set; }
    
    public Guid? AnswerId { get; set; }
    
    public TestAnswer? Answer { get; set; }
    
    public bool? IsCorrect { get; set; }
}