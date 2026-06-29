using PetWorld.Domain.SharedKernel;

namespace PetWorld.Domain.Chat;

public sealed class ChatInteraction
{
    public Guid Id { get; }
    public CustomerQuestion Question { get; }
    public AssistantAnswer Answer { get; }
    public IterationCount IterationCount { get; }
    public DateTime CreatedAt { get; }

    private ChatInteraction(Guid id, CustomerQuestion question, AssistantAnswer answer, IterationCount iterationCount, DateTime createdAt)
    {
        Id = id;
        Question = question;
        Answer = answer;
        IterationCount = iterationCount;
        CreatedAt = createdAt;
    }

    public static ChatInteraction Create(string question, string answer, int iterationCount)
    {
        return new ChatInteraction(
            Guid.NewGuid(),
            CustomerQuestion.Create(question),
            AssistantAnswer.Create(answer),
            IterationCount.Create(iterationCount),
            DateTime.UtcNow);
    }
}
