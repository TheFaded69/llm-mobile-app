namespace GameModes.Infrastructure.Entities;

public class TestSetEntity
{
    public string Id { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public string DurationLabel { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int QuestionCount { get; set; }
    public int ProgressPercent { get; set; }
    public string SectionDate { get; set; } = string.Empty;
    public int TotalInCourse { get; set; }
    public List<string>? SetAnswerPool { get; set; }

    public List<QuestionEntity> Questions { get; set; } = [];
}
