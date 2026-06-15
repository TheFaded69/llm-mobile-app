namespace Main.Contract.Dialogs.V1.DTO;

public class DialogDTO
{
    public Guid DialogId { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid TutorId { get; set; }
    
    public List<MessageDTO> Messages { get; set; }
}