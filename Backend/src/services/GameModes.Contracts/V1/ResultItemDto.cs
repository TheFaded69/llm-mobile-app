namespace GameModes.Contracts.V1;

public class ResultItemDto
{
    public string Kind { get; set; } = string.Empty;
    public string QuestionId { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string ExplainTerm { get; set; } = string.Empty;
    public string ExplainText { get; set; } = string.Empty;
    public string? Term { get; set; }
    public string? TermTranslation { get; set; }
    public bool? IsPairingCorrect { get; set; }
    public bool? UserSaidTrue { get; set; }
    public List<string>? Options { get; set; }
    public int? CorrectIndex { get; set; }
    public int? SelectedIndex { get; set; }
    public string? CorrectLabel { get; set; }
    public string? SelectedLabel { get; set; }
    public string? CorrectText { get; set; }
    public string? UserText { get; set; }
}
