using PetWorld.Domain.SharedKernel;

namespace PetWorld.Domain.Chat;

public sealed record AssistantAnswer
{
    public string Value { get; }

    private AssistantAnswer(string value)
    {
        Value = value;
    }

    public static AssistantAnswer Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("AssistantAnswer.Empty", "Answer must not be empty.");

        return new AssistantAnswer(value);
    }
}
