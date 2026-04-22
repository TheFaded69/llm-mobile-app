namespace GameModes.Infrastructure.Entities;

public class SessionAnswerEntity
{
    public string Id { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public string QuestionId { get; set; } = string.Empty;
    public bool? UserSaidTrue { get; set; }
    public int? SelectedIndex { get; set; }
    public string? SelectedLabel { get; set; }
    public string? UserText { get; set; }

    public TestSessionEntity Session { get; set; } = null!;
}
