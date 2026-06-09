using System.ComponentModel.DataAnnotations;

namespace Main.Contract.Dialogs.V1.Requests;

public class AddDialogRequest
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid TutorId { get; set; }
}