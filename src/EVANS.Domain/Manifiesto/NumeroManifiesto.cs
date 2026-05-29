using System.Text.RegularExpressions;

namespace EVANS.Domain.Manifiesto;

public record NumeroManifiesto
{
    private static readonly Regex FormatoRegex = new(@"^\d{4}-\d+$", RegexOptions.Compiled);

    public string Value { get; }

    public NumeroManifiesto(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !FormatoRegex.IsMatch(value))
            throw new ArgumentException("NUMERO_FORMATO_INVALIDO", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}
