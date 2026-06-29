using PetWorld.Application.Chat.Models;

namespace PetWorld.Application.Chat.Abstractions;

public interface IGetChatHistoryHandler
{
    Task<IReadOnlyList<ChatHistoryItem>> HandleAsync(CancellationToken cancellationToken);
}
