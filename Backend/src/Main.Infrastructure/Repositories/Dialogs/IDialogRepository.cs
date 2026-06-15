using Main.Domain.Dialogs.Models;

namespace Main.Infrastructure.Repositories.Dialogs;

public interface IDialogRepository
{
    Task<Guid> CreateDialog(Guid userId, Guid tutorId, CancellationToken cancellationToken);
    
    Task<Dialog> GetDialog(Guid dialogId, CancellationToken cancellationToken);
    
    Task<List<Dialog>> GetDialogsWithLastMessage(Guid userId, CancellationToken cancellationToken);
    
    Task DeleteDialog(Guid dialogId, CancellationToken cancellationToken);
    
    Task UpdateDialog(Dialog dialog, CancellationToken cancellationToken);
    
    Task AddMessage(Guid dialogId, Message message, CancellationToken cancellationToken);
}