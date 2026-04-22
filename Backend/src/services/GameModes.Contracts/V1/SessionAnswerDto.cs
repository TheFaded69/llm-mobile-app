namespace GameModes.Contracts.V1;

public class SessionAnswerDto
{
    public string Kind { get; set; } = string.Empty;
    public string QuestionId { get; set; } = string.Empty;
    public bool? UserSaidTrue { get; set; }
    public int? SelectedIndex { get; set; }
    public string? SelectedLabel { get; set; }
    public string? UserText { get; set; }
}
