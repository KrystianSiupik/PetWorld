using PetWorld.Domain.Chat;

namespace PetWorld.Application.Chat.Abstractions;

public interface IChatRepository
{
    Task AddAsync(ChatInteraction interaction, CancellationToken cancellationToken);

    Task<IReadOnlyList<ChatInteraction>> GetAllAsync(CancellationToken cancellationToken);
}
