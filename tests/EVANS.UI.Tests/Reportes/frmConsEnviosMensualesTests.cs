using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Ports;
using EVANS.Application.Reportes.Queries;
using EVANS.UI.WinForms.Reportes;
using MediatR;

namespace EVANS.UI.Tests.Reportes;

public sealed class FrmConsEnviosMensualesTests
{
    [WinFormsFact]
    public async Task CargarDestinosAsync_DisplaysActiveDestinations()
    {
        var mediator = Substitute.For<IMediator>();
        var repository = Substitute.For<IReportesConsultaRepository>();
        var exporter = Substitute.For<IEnviosMensualesExcelExporter>();
        repository.ListarDestinosActivosAsync(Arg.Any<CancellationToken>())
            .Returns([new DestinoReporteDto(1, "Lima"), new DestinoReporteDto(2, "Arequipa")]);

        using var form = new frmConsEnviosMensuales(mediator, repository, exporter, 2024);

        await form.CargarDestinosAsync();

        form.DestinoCount.Should().Be(2);
    }

    [WinFormsFact]
    public async Task BuscarAsync_WithSelectedDestination_SendsQueryAndEnablesExport()
    {
        var mediator = Substitute.For<IMediator>();
        var repository = Substitute.For<IReportesConsultaRepository>();
        var exporter = Substitute.For<IEnviosMensualesExcelExporter>();
        repository.ListarDestinosActivosAsync(Arg.Any<CancellationToken>())
            .Returns([new DestinoReporteDto(1, "Lima")]);
        mediator.Send(Arg.Any<ConsultarEnviosMensualesQuery>(), Arg.Any<CancellationToken>())
            .Returns([new EnvioMensualDto("Cliente Uno", 3, new DateTime(2024, 6, 20))]);

        using var form = new frmConsEnviosMensuales(mediator, repository, exporter, 2024);
        await form.CargarDestinosAsync();
        form.SetTestDateRange(new DateTime(2024, 6, 1), new DateTime(2024, 6, 30));
        form.SelectTestDestinos(1);

        var success = await form.BuscarAsync();

        success.Should().BeTrue();
        form.ResultCount.Should().Be(1);
        form.FirstCliente.Should().Be("Cliente Uno");
        form.ExportEnabled.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<ConsultarEnviosMensualesQuery>(query =>
                query.Filtro.Year == 2024 &&
                query.Filtro.FechaDesde == new DateTime(2024, 6, 1) &&
                query.Filtro.FechaHasta == new DateTime(2024, 6, 30) &&
                query.Filtro.DestinoCodigos.SequenceEqual(new[] { 1 })),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task BuscarAsync_WithoutSelectedDestination_ShowsInlineError()
    {
        var mediator = Substitute.For<IMediator>();
        var repository = Substitute.For<IReportesConsultaRepository>();
        var exporter = Substitute.For<IEnviosMensualesExcelExporter>();
        repository.ListarDestinosActivosAsync(Arg.Any<CancellationToken>())
            .Returns([new DestinoReporteDto(1, "Lima")]);

        using var form = new frmConsEnviosMensuales(mediator, repository, exporter, 2024);
        await form.CargarDestinosAsync();

        var success = await form.BuscarAsync();

        success.Should().BeFalse();
        form.ErrorMessage.Should().Be("Seleccione al menos un destino.");
        await mediator.DidNotReceive().Send(Arg.Any<ConsultarEnviosMensualesQuery>(), Arg.Any<CancellationToken>());
    }
}
