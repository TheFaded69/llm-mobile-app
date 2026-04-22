namespace GameModes.Contracts.V1;

public static class TestModes
{
    public const string TrueFalse = "true-false";
    public const string Questions = "questions";
    public const string Selection = "selection";
    public const string Written = "written";

    public static readonly HashSet<string> All =
    [
        TrueFalse,
        Questions,
        Selection,
        Written
    ];
}
