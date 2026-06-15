using BaseInfrastructure.Factories;
using Main.Domain.Dialogs.Models;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Repositories.Dialogs;

public class DialogRepository : IDialogRepository
{
    private readonly IRepositoryFactory<Dialog, Guid> _dialogRepositoryFactory;

    public DialogRepository(IRepositoryFactory<Dialog, Guid> dialogRepositoryFactory)
    {
        _dialogRepositoryFactory = dialogRepositoryFactory;
    }

    public async Task<Guid> CreateDialog(Guid userId, Guid tutorId, CancellationToken cancellationToken)
    {
        using var repository = await _dialogRepositoryFactory.CreateRepositoryAsync();

        var dialog = new Dialog()
        {
            UserId = userId,
            TutorId = tutorId,
        };
        
        repository.Insert(dialog);
        
        await repository.CommitAsync();
        
        return dialog.Id;
    }

    public async Task<Dialog> GetDialog(Guid dialogId, CancellationToken cancellationToken)
    {
        using var repository = await _dialogRepositoryFactory.CreateRepositoryAsync();
        
        return await repository
            .Query
            .Include(d => d.Messages.OrderBy(m => m.CreateTime))
            .FirstOrDefaultAsync(d => d.Id == dialogId, cancellationToken);
    }

    public async Task<List<Dialog>> GetDialogsWithLastMessage(Guid userId, CancellationToken cancellationToken)
    {
        using var repository = await _dialogRepositoryFactory.CreateRepositoryAsync();
        
        return await repository.Query
            .Where(d => d.UserId == userId)
            .Select(d => new Dialog
            {
                Id = d.Id,
                UserId = d.UserId,
                TutorId = d.TutorId,

                Messages = d.Messages
                    .OrderByDescending(m => m.CreateTime)
                    .Take(1)
                    .ToList()
            })
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteDialog(Guid dialogId, CancellationToken cancellationToken)
    { 
        using var repository = await _dialogRepositoryFactory.CreateRepositoryAsync();
        
        var dialog = await repository.GetAsync(dialogId, cancellationToken) ?? throw new Exception($"Такого диалога [{dialogId}] не существует");
        
        repository.Delete(dialog);
        
        await repository.CommitAsync();
    }

    public async Task UpdateDialog(Dialog dialog, CancellationToken cancellationToken)
    {
        using var repository = await _dialogRepositoryFactory.CreateRepositoryAsync();
        
        repository.Update(dialog);
        
        await repository.CommitAsync();
    }

    public async Task AddMessage(Guid dialogId, Message message, CancellationToken cancellationToken)
    {
        using var repository = await _dialogRepositoryFactory.CreateRepositoryAsync();

        var dialog = await repository.GetAsync(dialogId, cancellationToken);

        if (dialog == null) return;
        
        dialog.Messages.Add(message);
        
        await repository.CommitAsync();
    }
}