using Main.Contract.Dialogs.V1.DTO;

namespace Main.Contract.Dialogs.V1.Responses;

public class GetDialogsResponse
{
    public List<DialogMetaDTO> Dialogs { get; set; }
}