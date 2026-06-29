using PetWorld.Domain.SharedKernel;

namespace PetWorld.Domain.Products;

public sealed class ProductCategory : SmartEnum<ProductCategory>
{
    public static readonly ProductCategory Dogs = new(nameof(Dogs), 1, "Psy");
    public static readonly ProductCategory Cats = new(nameof(Cats), 2, "Koty");
    public static readonly ProductCategory Birds = new(nameof(Birds), 3, "Ptaki");
    public static readonly ProductCategory Fish = new(nameof(Fish), 4, "Ryby");
    public static readonly ProductCategory SmallPets = new(nameof(SmallPets), 5, "Małe zwierzęta");
    public static readonly ProductCategory Reptiles = new(nameof(Reptiles), 6, "Gady");

    public string DisplayName { get; }

    private ProductCategory(string name, int value, string displayName) : base(name, value)
    {
        DisplayName = displayName;
    }
}
