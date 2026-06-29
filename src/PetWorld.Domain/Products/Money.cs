using PetWorld.Domain.SharedKernel;

namespace PetWorld.Domain.Products;

public sealed record Money
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, Currency currency)
    {
        if (currency is null)
            throw new DomainException("Money.CurrencyRequired", "Currency is required.");

        if (amount < 0)
            throw new DomainException("Money.Negative", "Amount must not be negative.");

        return new Money(amount, currency);
    }
}
