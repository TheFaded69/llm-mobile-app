namespace GameModes.Contracts.V1;

public class TestModeMeta
{
    public string Mode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool SupportsPerQuestionFeedback { get; set; }
    public bool SupportsFinalSubmitOnly { get; set; }
}
