using System.ComponentModel.DataAnnotations;

namespace Main.Contract.Dialogs.V1.Requests;

public class GetDialogRequest
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid TutorId { get; set; }
    
    public Guid DialogId { get; set; }
}