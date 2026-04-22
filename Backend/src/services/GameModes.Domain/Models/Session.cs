using BaseDomain.Models;

namespace GameModes.Domain.Models;

public class Session : GuidEntity
{
    public Game CurrentGame { get; set; }
    
    public DateTime StartTime { get; set; }
}