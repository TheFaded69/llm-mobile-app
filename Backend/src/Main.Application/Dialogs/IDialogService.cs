using Main.Contract.Dialogs.V1.Requests;
using Main.Contract.Dialogs.V1.Responses;

namespace Main.Application.Dialogs;

public interface IDialogService
{
    Task<GetDialogsResponse> GetDialogs(Guid userId, CancellationToken cancellationToken);
    
    Task<GetDialogResponse> GetDialog(Guid dialogId, CancellationToken cancellationToken);

    Task<Guid> CreateDialog(AddDialogRequest request, CancellationToken cancellationToken);
    
    Task UpdateDialog(UpdateDialogRequest request, Guid dialogId, CancellationToken cancellationToken);
    
    Task<AddMessageResponse> AddMessage(AddMessageRequest request, CancellationToken cancellationToken);
    
    Task DeleteDialog(Guid dialogId, CancellationToken cancellationToken);
}