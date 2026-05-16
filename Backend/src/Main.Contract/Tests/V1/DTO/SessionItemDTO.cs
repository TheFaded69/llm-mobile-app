namespace Main.Contract.Tests.V1.DTO;

public class SessionItemDTO
{
    public Guid Id { get; set; }
    
    public Guid SessionId { get; set; }
    
    public QuestionDTO Question { get; set; }
    
    public AnswerDTO Answer { get; set; }
    
    public bool? IsCorrect { get; set; }
}