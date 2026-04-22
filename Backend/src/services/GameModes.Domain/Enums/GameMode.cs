using System.ComponentModel;

namespace GameModes.Domain.Enums;

public enum GameMode
{
    None = 0,
    
    [Description("true-false")]
    TrueFalse = 1,
    
    [Description("questions")]
    Questions = 2,
    
    [Description("selection")]
    Selection = 3,
    
    [Description("written")]
    Written = 4
}