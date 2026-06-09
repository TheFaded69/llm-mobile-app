using System.ComponentModel.DataAnnotations;
using Main.Contract.Dialogs.V1.DTO;

namespace Main.Contract.Dialogs.V1.Responses;

public class GetDialogResponse
{
    public DialogDTO Dialog { get; set; }
}