namespace EVANS.Domain.Shared;

public readonly record struct Ruc
{
    private const int RequiredLength = 11;

    private Ruc(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static bool TryCreate(string? value, out Ruc ruc)
    {
        ruc = default;

        if (value is null || value.Length != RequiredLength)
            return false;

        for (var i = 0; i < value.Length; i++)
        {
            if (!char.IsDigit(value[i]))
                return false;
        }

        ruc = new Ruc(value);
        return true;
    }

    public static Ruc Parse(string value) =>
        TryCreate(value, out var ruc)
            ? ruc
            : throw new ArgumentException($"Invalid RUC: '{value}'.", nameof(value));

    public override string ToString() => Value;

    public static implicit operator string(Ruc ruc) => ruc.Value;
}
