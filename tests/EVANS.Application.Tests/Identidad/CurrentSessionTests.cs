using EVANS.Application.Identidad.DTOs;
using EVANS.Application.Identidad.Services;
using EVANS.Application.Shared.DTOs;

namespace EVANS.Application.Tests.Identidad;

public sealed class CurrentSessionTests
{
    [Fact]
    public void Start_StoresAuthenticatedUserParametersAndYear()
    {
        var session = new CurrentSession();
        var usuario = new UsuarioSesionDto("testuser", "Test User", true);
        var parametros = BuildParametros();

        session.Start(usuario, parametros, 2026);

        session.IsAuthenticated.Should().BeTrue();
        session.Current.Should().NotBeNull();
        session.Current!.Usuario.NombreUsuario.Should().Be("testuser");
        session.Current.Parametros.IgvRate.Should().Be(0.18m);
        session.Current.Year.Should().Be(2026);
    }

    [Fact]
    public void Clear_RemovesCurrentSession()
    {
        var session = new CurrentSession();
        session.Start(new UsuarioSesionDto("testuser", "Test User", false), BuildParametros(), 2026);

        session.Clear();

        session.IsAuthenticated.Should().BeFalse();
        session.Current.Should().BeNull();
    }

    private static ParametrosDto BuildParametros() => new(
        IgvRate: 0.18m,
        FacturaSerie: "F001",
        FacturaNro1: "000001",
        FacturaNro2: "000000",
        BoletaSerie: "B001",
        BoletaNro1: "000001",
        BoletaNro2: "000000",
        GuiaRemisionSerie: "T001",
        GuiaRemisionNro1: "000001",
        GuiaRemisionNro2: "000000",
        Manifiesto: "00000000001",
        Remitente: "EVANS Cargo",
        EmailRemitente: "ops@evans.test",
        PassRemitente: "secret",
        Smtp: "smtp.evans.test",
        Puerto: 587);
}
