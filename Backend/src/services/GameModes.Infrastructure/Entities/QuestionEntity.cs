namespace GameModes.Infrastructure.Entities;

public class QuestionEntity
{
    public string Id { get; set; } = string.Empty;
    public string SetId { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public string QuestionId { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string ExplainTerm { get; set; } = string.Empty;
    public string ExplainText { get; set; } = string.Empty;
    public string? Term { get; set; }
    public string? TermTranslation { get; set; }
    public List<string>? Options { get; set; }
    public List<string>? AnswerPool { get; set; }

    public TestSetEntity Set { get; set; } = null!;
}
