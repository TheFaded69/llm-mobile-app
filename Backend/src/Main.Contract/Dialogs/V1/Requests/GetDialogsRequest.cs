using System.ComponentModel.DataAnnotations;

namespace Main.Contract.Dialogs.V1.Requests;

public class GetDialogsRequest
{
    [Required]
    public Guid UserId { get; set; }
}