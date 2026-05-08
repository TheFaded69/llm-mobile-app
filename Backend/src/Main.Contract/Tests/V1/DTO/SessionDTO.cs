using Main.Domain.Tests.Enums;

namespace Main.Contract.Tests.V1.DTO;

public class SessionDTO
{
    public Guid Id { get; set; }
    
    public Guid SetId { get; set; }
    
    public TestMode TestMode { get; set; }
    
    public SessionStatus  SessionStatus { get; set; }
    
    public int Progress { get; set; }
    
    public List<SessionItemDTO> SessionItems { get; set; }
}