namespace EVANS.Domain.GuiaRemision;

public record NumeroGuia
{
    public string Serie { get; }
    public int Numero { get; }

    public NumeroGuia(string serie, int numero)
    {
        if (string.IsNullOrWhiteSpace(serie) || serie.Length != 4)
            throw new ArgumentException("Serie must be exactly 4 characters.", nameof(serie));

        if (numero < 0)
            throw new ArgumentException("Numero must be zero or positive.", nameof(numero));

        Serie = serie;
        Numero = numero;
    }

    public override string ToString() => $"{Serie}-{Numero:D6}";

    public static NumeroGuia Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Cannot parse empty value.", nameof(value));

        var dashIndex = value.IndexOf('-');
        if (dashIndex < 0)
            throw new ArgumentException($"Invalid NumeroGuia format: '{value}'.", nameof(value));

        var serie = value[..dashIndex];
        var numeroStr = value[(dashIndex + 1)..];

        if (!int.TryParse(numeroStr, out var numero))
            throw new ArgumentException($"Invalid numero in '{value}'.", nameof(value));

        return new NumeroGuia(serie, numero);
    }
}
