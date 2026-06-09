using Main.Contract.Dialogs.V1.Requests;
using Main.Contract.Dialogs.V1.Responses;

namespace Main.Application.Dialogs;

public class DialogService : IDialogService
{
    public async Task<GetDialogsResponse> GetDialogs(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<GetDialogResponse> GetDialog(Guid dialogId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task CreateDialog(AddDialogRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateDialog(UpdateDialogRequest request, Guid dialogId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<AddMessageResponse> AddMessage(AddMessageRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteDialog(Guid dialogId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}