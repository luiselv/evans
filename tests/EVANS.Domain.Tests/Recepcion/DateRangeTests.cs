using FluentAssertions;
using Xunit;
using DomEx = EVANS.Domain.Recepcion.DomainException;
using DR = EVANS.Domain.Recepcion.DateRange;

namespace EVANS.Domain.Tests.Recepcion;

public class DateRangeTests
{
    // DR001-Hoy: factory produces single-day range
    [Fact]
    public void Hoy_ReturnsInicioPlusEndOfDay()
    {
        var range = DR.Hoy();
        range.Inicio.Should().Be(DateTime.Today);
        range.Fin.Should().Be(DateTime.Today.AddDays(1).AddTicks(-1));
    }

    // DR001-MesActual: factory covers full calendar month
    [Fact]
    public void MesActual_CubresDesdeElPrimeroHastaElUltimo()
    {
        var range = DR.MesActual();
        var hoy = DateTime.Today;
        var primero = new DateTime(hoy.Year, hoy.Month, 1);
        var ultimo = primero.AddMonths(1).AddTicks(-1);

        range.Inicio.Should().Be(primero);
        range.Fin.Should().Be(ultimo);
    }

    // DR001-Intervalo: rejects inverted range
    [Fact]
    public void Intervalo_InvertedRange_ThrowsDomainException()
    {
        var inicio = new DateTime(2026, 5, 10);
        var fin = new DateTime(2026, 5, 1);

        var act = () => DR.Intervalo(inicio, fin);

        act.Should().Throw<DomEx>()
            .Which.Code.Should().Be("DR001");
    }

    // DR001-Intervalo: valid range succeeds
    [Fact]
    public void Intervalo_ValidRange_SetsInicioAndFin()
    {
        var inicio = new DateTime(2026, 5, 1);
        var fin = new DateTime(2026, 5, 15);

        var range = DR.Intervalo(inicio, fin);

        range.Inicio.Should().Be(inicio.Date);
        range.Fin.Should().Be(fin.Date.AddDays(1).AddTicks(-1));
    }
}
