namespace Main.Contract.Dialogs.V1.DTO;

public class DialogMetaDTO
{
    public Guid DialogId { get; set; }
    
    public Guid TutorId { get; set; }
    
    public Guid UserId { get; set; }
    
    public MessageDTO LastMessage { get; set; }
}