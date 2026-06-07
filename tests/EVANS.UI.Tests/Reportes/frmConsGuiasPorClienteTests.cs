using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Ports;
using EVANS.Application.Reportes.Queries;
using EVANS.UI.WinForms.Reportes;
using MediatR;

namespace EVANS.UI.Tests.Reportes;

public sealed class FrmConsGuiasPorClienteTests
{
    [WinFormsFact]
    public void Constructor_MatchesLegacyWindowMetadata()
    {
        var mediator = Substitute.For<IMediator>();
        var repository = Substitute.For<IReportesConsultaRepository>();

        using var form = new frmConsGuiasPorCliente(mediator, repository, 2024);

        form.ClientSize.Should().Be(new Size(821, 602));
        form.Text.Should().Be("Consulta de Guias por Cliente");
    }

    [WinFormsFact]
    public async Task CargarClientesAsync_DisplaysClientsAndIdentification()
    {
        var mediator = Substitute.For<IMediator>();
        var repository = Substitute.For<IReportesConsultaRepository>();
        repository.ListarClientesAsync(Arg.Any<CancellationToken>())
            .Returns([new ClienteReporteDto(1, "Cliente Uno", "20111111111")]);

        using var form = new frmConsGuiasPorCliente(mediator, repository, 2024);

        await form.CargarClientesAsync();
        form.SetTestCliente(1);

        form.ClienteCount.Should().Be(1);
        form.NumeroIdentificacion.Should().Be("20111111111");
    }

    [WinFormsFact]
    public async Task BuscarAsync_WithSelectedClient_SendsQueryAndBindsRows()
    {
        var mediator = Substitute.For<IMediator>();
        var repository = Substitute.For<IReportesConsultaRepository>();
        repository.ListarClientesAsync(Arg.Any<CancellationToken>())
            .Returns([new ClienteReporteDto(2, "Cliente Dos", "20222222222")]);
        mediator.Send(Arg.Any<ConsultarGuiasPorClienteQuery>(), Arg.Any<CancellationToken>())
            .Returns([new GuiaPorClienteDto(
                13,
                "GR01-000013",
                "Cliente Uno",
                "Cliente Dos",
                new DateTime(2024, 6, 15),
                new DateTime(2024, 6, 16),
                5,
                130m,
                false)]);

        using var form = new frmConsGuiasPorCliente(mediator, repository, 2024);
        await form.CargarClientesAsync();
        form.SetTestCliente(2);
        form.SetTestDateRange(new DateTime(2024, 6, 1), new DateTime(2024, 6, 30));
        form.SetTestPendientes(true);

        var success = await form.BuscarAsync();

        success.Should().BeTrue();
        form.ResultCount.Should().Be(1);
        form.FirstNroDoc.Should().Be("GR01-000013");
        await mediator.Received(1).Send(
            Arg.Is<ConsultarGuiasPorClienteQuery>(query =>
                query.Filtro.Year == 2024 &&
                query.Filtro.ClienteCodigo == 2 &&
                query.Filtro.FechaDesde == new DateTime(2024, 6, 1) &&
                query.Filtro.FechaHasta == new DateTime(2024, 6, 30) &&
                query.Filtro.SoloPendientes),
            Arg.Any<CancellationToken>());
    }
}
