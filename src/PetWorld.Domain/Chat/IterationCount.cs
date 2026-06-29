using PetWorld.Domain.SharedKernel;

namespace PetWorld.Domain.Chat;

public sealed record IterationCount
{
    public int Value { get; }

    private IterationCount(int value)
    {
        Value = value;
    }

    public static IterationCount Create(int value)
    {
        if (value < 1)
            throw new DomainException("IterationCount.TooLow", "Iteration count must be at least 1.");

        return new IterationCount(value);
    }
}
