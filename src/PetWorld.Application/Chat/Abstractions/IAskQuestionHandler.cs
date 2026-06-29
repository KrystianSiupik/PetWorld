using PetWorld.Application.Chat.Models;

namespace PetWorld.Application.Chat.Abstractions;

public interface IAskQuestionHandler
{
    Task<ChatResult> HandleAsync(string question, CancellationToken cancellationToken);
}
