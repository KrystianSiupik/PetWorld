using PetWorld.Domain.Chat;

namespace PetWorld.Application.Chat.Abstractions;

public interface IWriterAgent
{
    Task<AssistantAnswer> WriteAsync(CustomerQuestion question, string? criticFeedback, CancellationToken cancellationToken);
}
