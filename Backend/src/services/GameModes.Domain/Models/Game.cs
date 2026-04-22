using BaseDomain.Models;
using GameModes.Domain.Enums;

namespace GameModes.Domain.Models;

public class Game : GuidEntity
{
    public string Name { get; set; }
    
    public GameMode GameMode { get; set; }
    
    public GameDifficult GameDifficult { get; set; }
    
    public int AverageDuration { get; set; }
}