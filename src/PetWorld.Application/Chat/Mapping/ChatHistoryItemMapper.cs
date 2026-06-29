using PetWorld.Application.Chat.Models;
using PetWorld.Domain.Chat;

namespace PetWorld.Application.Chat.Mapping;

public static class ChatHistoryItemMapper
{
    public static ChatHistoryItem Map(ChatInteraction interaction) =>
        new(
            interaction.Id,
            interaction.CreatedAt,
            interaction.Question.Value,
            interaction.Answer.Value,
            interaction.IterationCount.Value);
}
