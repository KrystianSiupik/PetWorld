using PetWorld.Application.Chat.Models;
using PetWorld.Domain.Chat;

namespace PetWorld.Application.Chat.Abstractions;

public interface ICriticAgent
{
    Task<CriticVerdict> ReviewAsync(CustomerQuestion question, AssistantAnswer answer, CancellationToken cancellationToken);
}
