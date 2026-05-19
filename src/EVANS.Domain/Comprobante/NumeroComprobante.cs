using System.Text.RegularExpressions;

namespace EVANS.Domain.Comprobante;

public record NumeroComprobante
{
    private static readonly Regex SerieRegex = new(@"^[A-Z]\d{3}$", RegexOptions.Compiled);
    private static readonly Regex NumeroRegex = new(@"^\d{6}$", RegexOptions.Compiled);

    public string Serie { get; }
    public string Numero { get; }

    public NumeroComprobante(string serie, string numero)
    {
        if (string.IsNullOrWhiteSpace(serie) || !SerieRegex.IsMatch(serie))
            throw new ArgumentException("SERIE_FORMATO_INVALIDO", nameof(serie));

        if (string.IsNullOrWhiteSpace(numero) || !NumeroRegex.IsMatch(numero))
            throw new ArgumentException("NUMERO_FORMATO_INVALIDO", nameof(numero));

        Serie = serie;
        Numero = numero;
    }

    public override string ToString() => $"{Serie}-{Numero}";
}
