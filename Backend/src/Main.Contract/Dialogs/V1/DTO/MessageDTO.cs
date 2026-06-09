using Main.Domain.Dialogs.Enums;

namespace Main.Contract.Dialogs.V1.DTO;

public class MessageDTO
{
    public string Text { get; set; }
    
    public string? Translation { get; set; }
    
    public MessageSender  Sender { get; set; }
    
    public DateTime CreateTime { get; set; }
}