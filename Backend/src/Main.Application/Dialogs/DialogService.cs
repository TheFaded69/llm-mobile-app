using Main.Contract.Dialogs.V1.DTO;
using Main.Contract.Dialogs.V1.Requests;
using Main.Contract.Dialogs.V1.Responses;
using Main.Domain.Dialogs.Enums;
using Main.Domain.Dialogs.Models;
using Main.Infrastructure.Repositories.Dialogs;

namespace Main.Application.Dialogs;

public class DialogService : IDialogService
{
    private readonly IDialogRepository _dialogRepository;

    public DialogService(IDialogRepository dialogRepository)
    {
        _dialogRepository = dialogRepository;
    }
    
    public async Task<GetDialogsResponse> GetDialogs(Guid userId, CancellationToken cancellationToken)
    {
        var dialogs = await _dialogRepository.GetDialogsWithLastMessage(userId, cancellationToken);
        return new GetDialogsResponse
        {
            Dialogs = dialogs.Select(dialog => new DialogMetaDTO
            {
                DialogId = dialog.Id,
                TutorId = dialog.TutorId,
                UserId = dialog.UserId,
                LastMessage = new MessageDTO
                {
                    Text = dialog.Messages.First().Text,
                    Translation = dialog.Messages.First().Translation,
                    Sender = dialog.Messages.First().Sender,
                    CreateTime = dialog.Messages.First().CreateTime
                }
            }).ToList()
        };
    }

    public async Task<GetDialogResponse> GetDialog(Guid dialogId, CancellationToken cancellationToken)
    {
        var dialog =  await _dialogRepository.GetDialog(dialogId, cancellationToken);

        return new GetDialogResponse
        {
            Dialog = new DialogDTO
            {
                DialogId = dialog.Id,
                UserId = dialog.UserId,
                TutorId = dialog.TutorId,
                Messages = dialog.Messages.Select(message => new MessageDTO
                {
                    Text = message.Text,
                    Translation = message.Translation,
                    Sender = message.Sender,
                    CreateTime = message.CreateTime
                }).ToList()
            }
        };
    }

    public async Task<Guid> CreateDialog(AddDialogRequest request, CancellationToken cancellationToken)
    {
        return await _dialogRepository.CreateDialog(request.UserId, request.TutorId, cancellationToken);
    }

    public async Task UpdateDialog(UpdateDialogRequest request, Guid dialogId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<AddMessageResponse> AddMessage(AddMessageRequest request, CancellationToken cancellationToken)
    {
        var message = new Message
        {
            DialogId = request.DialogId,
            Sender = MessageSender.FromUser,
            Text = request.Message,
            Translation = null
        };
        
        await _dialogRepository.AddMessage(request.DialogId, message, cancellationToken);
        
        //todo AI ANSWER
        
        var responseMessage = new Message
        {
            DialogId = request.DialogId,
            Sender = MessageSender.ToUser,
            Text = $"Test callback for message [dialogId:{request.DialogId}], message [text]: {request.Message}",
            Translation = "Test translation"
        };
        
        await _dialogRepository.AddMessage(request.DialogId, responseMessage, cancellationToken);
        
        return new AddMessageResponse
        {
            Message = new MessageDTO
            {
                Text = responseMessage.Text,
                Translation = responseMessage.Translation,
                Sender = MessageSender.ToUser,
                CreateTime = responseMessage.CreateTime
            }
        };
    }

    public async Task DeleteDialog(Guid dialogId, CancellationToken cancellationToken)
    {
        await _dialogRepository.DeleteDialog(dialogId, cancellationToken);
    }
}