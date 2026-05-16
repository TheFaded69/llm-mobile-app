using Main.Domain.Tests.Enums;

namespace Main.Contract.Tests.V1.DTO;

public class SetDTO
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public TestDifficult TestDifficult { get; set; }
    
    public int Duration { get; set; }
    
    public List<SetItemDTO> SetItems { get; set; }
    
    public List<SessionDTO> Sessions { get; set; }
}