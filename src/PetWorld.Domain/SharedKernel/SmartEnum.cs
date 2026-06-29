using System.Reflection;

namespace PetWorld.Domain.SharedKernel;

public abstract class SmartEnum<TEnum> : IEquatable<SmartEnum<TEnum>>
    where TEnum : SmartEnum<TEnum>
{
    private static readonly Lazy<IReadOnlyList<TEnum>> Items = new(() =>
        typeof(TEnum)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(field => typeof(TEnum).IsAssignableFrom(field.FieldType))
            .Select(field => (TEnum)field.GetValue(null)!)
            .ToList());

    public string Name { get; }
    public int Value { get; }

    protected SmartEnum(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public static IReadOnlyList<TEnum> List() => Items.Value;

    public static TEnum? FromName(string name) =>
        Items.Value.FirstOrDefault(item => item.Name == name);

    public static TEnum? FromValue(int value) =>
        Items.Value.FirstOrDefault(item => item.Value == value);

    public bool Equals(SmartEnum<TEnum>? other) => other is not null && Value == other.Value;

    public override bool Equals(object? obj) => obj is SmartEnum<TEnum> other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Name;
}
