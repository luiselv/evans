namespace EVANS.Domain.Recepcion;

/// <summary>
/// Immutable value object representing a date interval [Inicio, Fin].
/// Invariant: Inicio &lt;= Fin (enforced at construction — DR001).
/// Factory methods: Hoy(), MesActual(), Intervalo(from, to).
/// </summary>
public readonly record struct DateRange
{
    public DateTime Inicio { get; }
    public DateTime Fin { get; }

    private DateRange(DateTime inicio, DateTime fin)
    {
        if (inicio > fin)
            throw new DomainException("DR001", "Rango de fechas invalido");
        Inicio = inicio;
        Fin = fin;
    }

    /// <summary>Returns a range covering today from 00:00:00.0000000 to 23:59:59.9999999.</summary>
    public static DateRange Hoy()
    {
        var hoy = DateTime.Today;
        return new DateRange(hoy, hoy.AddDays(1).AddTicks(-1));
    }

    /// <summary>Returns a range covering the current calendar month (first day to last tick of last day).</summary>
    public static DateRange MesActual()
    {
        var hoy = DateTime.Today;
        var primero = new DateTime(hoy.Year, hoy.Month, 1);
        var ultimo = primero.AddMonths(1).AddTicks(-1);
        return new DateRange(primero, ultimo);
    }

    /// <summary>
    /// Creates a range from <paramref name="inicio"/>.Date to end-of-day of <paramref name="fin"/>.Date.
    /// Throws <see cref="DomainException"/> "DR001" if inicio &gt; fin.
    /// </summary>
    public static DateRange Intervalo(DateTime inicio, DateTime fin)
        => new DateRange(inicio.Date, fin.Date.AddDays(1).AddTicks(-1));
}
