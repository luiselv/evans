using EVANS.Domain.Catalogo;
using EVANS.Domain.Shared;
using FluentAssertions;

namespace EVANS.Domain.Tests.Catalogo;

public class EmpresaTests
{
    [Fact]
    public void Crear_ValidData_SetsActiveStateAndRuc()
    {
        Ruc.TryCreate("20123456789", out var ruc).Should().BeTrue();

        var empresa = Empresa.Crear("TRANSPORT SA", "Av Lima", "123", ruc, esPropia: true);

        empresa.RazonSocial.Should().Be("TRANSPORT SA");
        empresa.Ruc.Value.Should().Be("20123456789");
        empresa.EsPropia.Should().BeTrue();
        empresa.EstadoCodigo.Should().Be(CatalogoEstado.Activo);
    }

    [Fact]
    public void Crear_WithEstadoCodigo_UsesSelectedState()
    {
        Ruc.TryCreate("20123456789", out var ruc).Should().BeTrue();

        var empresa = Empresa.Crear("TRANSPORT SA", "Av Lima", "123", ruc, esPropia: true, CatalogoEstado.Inactivo);

        empresa.EstadoCodigo.Should().Be(CatalogoEstado.Inactivo);
    }

    [Fact]
    public void Actualizar_WithEstadoCodigo_UpdatesSelectedState()
    {
        Ruc.TryCreate("20123456789", out var ruc).Should().BeTrue();
        Ruc.TryCreate("20987654321", out var updatedRuc).Should().BeTrue();
        var empresa = Empresa.Crear("TRANSPORT SA", "Av Lima", "123", ruc, esPropia: true);

        empresa.Actualizar("TRANSPORTES SAC", "Av Norte", "555", updatedRuc, esPropia: false, CatalogoEstado.Inactivo);

        empresa.EstadoCodigo.Should().Be(CatalogoEstado.Inactivo);
        empresa.RazonSocial.Should().Be("TRANSPORTES SAC");
        empresa.Ruc.Value.Should().Be("20987654321");
        empresa.EsPropia.Should().BeFalse();
    }

    [Fact]
    public void Crear_BlankRazonSocial_ThrowsDomainException()
    {
        Ruc.TryCreate("20123456789", out var ruc).Should().BeTrue();

        Action act = () => Empresa.Crear(" ", null, null, ruc, esPropia: false);

        act.Should().Throw<DomainException>().Where(e => e.Code == "CAT-EMP-001");
    }

    [Fact]
    public void Deactivate_SetsInactiveState()
    {
        Ruc.TryCreate("20123456789", out var ruc).Should().BeTrue();
        var empresa = Empresa.Crear("TRANSPORT SA", null, null, ruc, esPropia: false);

        empresa.Deactivate();

        empresa.EstadoCodigo.Should().Be(CatalogoEstado.Inactivo);
    }
}
