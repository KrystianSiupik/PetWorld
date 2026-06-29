using Microsoft.EntityFrameworkCore;
using PetWorld.Application.Chat.Abstractions;
using PetWorld.Domain.Chat;

namespace PetWorld.Infrastructure.Persistence.Repositories;

public sealed class ChatRepository : IChatRepository
{
    private readonly PetWorldDbContext _dbContext;

    public ChatRepository(PetWorldDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ChatInteraction interaction, CancellationToken cancellationToken)
    {
        await _dbContext.ChatInteractions.AddAsync(interaction, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ChatInteraction>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.ChatInteractions
            .AsNoTracking()
            .OrderByDescending(interaction => interaction.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
