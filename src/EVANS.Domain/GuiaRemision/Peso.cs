namespace EVANS.Domain.GuiaRemision;

public record Peso
{
    public decimal Valor { get; }

    public Peso(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentOutOfRangeException(nameof(valor), "Peso cannot be negative.");

        Valor = valor;
    }
}
