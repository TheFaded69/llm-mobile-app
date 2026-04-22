namespace GameModes.Infrastructure.Entities;

public class GameModeEntity
{
    public string Mode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool SupportsPerQuestionFeedback { get; set; }
    public bool SupportsFinalSubmitOnly { get; set; }
}
