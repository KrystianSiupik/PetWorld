using PetWorld.Domain.SharedKernel;

namespace PetWorld.Domain.Products;

public sealed class Currency : SmartEnum<Currency>
{
    public static readonly Currency Pln = new(nameof(Pln), 1, "PLN", "zł");

    public string Code { get; }
    public string Symbol { get; }

    private Currency(string name, int value, string code, string symbol) : base(name, value)
    {
        Code = code;
        Symbol = symbol;
    }
}
