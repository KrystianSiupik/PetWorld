using PetWorld.Application.Chat.Abstractions;
using PetWorld.Application.Chat.Mapping;
using PetWorld.Application.Chat.Models;

namespace PetWorld.Application.Chat.Handlers;

public sealed class GetChatHistoryHandler : IGetChatHistoryHandler
{
    private readonly IChatRepository _repository;

    public GetChatHistoryHandler(IChatRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ChatHistoryItem>> HandleAsync(CancellationToken cancellationToken)
    {
        var interactions = await _repository.GetAllAsync(cancellationToken);

        return interactions
            .Select(ChatHistoryItemMapper.Map)
            .ToList();
    }
}
