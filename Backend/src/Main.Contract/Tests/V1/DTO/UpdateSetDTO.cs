using Main.Domain.Tests.Enums;

namespace Main.Contract.Tests.V1.DTO;

public class UpdateSetDTO
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public TestDifficult TestDifficult { get; set; }
    
    public SetStatus SetStatus { get; set; }
    
    public int Duration { get; set; }
    
    public bool IsPublic { get; set; }
    
    public List<SetItemDTO> SetItems { get; set; }
}