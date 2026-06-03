using BaseDomain.Models;
using Main.Domain.Dialogs.Enums;

namespace Main.Domain.Dialogs.Models;

public class Message : GuidEntity
{
    public Guid DialogId { get; set; }
    
    public MessageSender Sender { get; set; }
    
    public string Text { get; set; }
    
    public string? Translation { get; set; }
}