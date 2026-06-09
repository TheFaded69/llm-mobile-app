using System.ComponentModel.DataAnnotations;

namespace Main.Contract.Dialogs.V1.Requests;

public class AddMessageRequest
{
    [Required]
    public Guid DialogId { get; set; }
    
    [Required]
    public string Message { get; set; }
}