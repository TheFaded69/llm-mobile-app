namespace GameModes.Contracts.V1;

public class TestFiltersResponse
{
    public List<string> Difficulties { get; set; } = [];
    public Dictionary<string, List<string>> CategoriesByMode { get; set; } = new();
    public List<string> SectionDates { get; set; } = [];
    public List<string> Sort { get; set; } = [];
}
