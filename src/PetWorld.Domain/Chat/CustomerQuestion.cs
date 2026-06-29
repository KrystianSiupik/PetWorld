using PetWorld.Domain.SharedKernel;

namespace PetWorld.Domain.Chat;

public sealed record CustomerQuestion
{
    public string Value { get; }

    private CustomerQuestion(string value)
    {
        Value = value;
    }

    public static CustomerQuestion Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("CustomerQuestion.Empty", "Question must not be empty.");

        return new CustomerQuestion(value);
    }
}
