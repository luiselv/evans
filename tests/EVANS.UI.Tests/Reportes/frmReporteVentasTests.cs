using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Ports;
using EVANS.Application.Reportes.Queries;
using EVANS.UI.WinForms.Reportes;
using MediatR;

namespace EVANS.UI.Tests.Reportes;

public sealed class FrmReporteVentasTests
{
    [WinFormsFact]
    public void Constructor_MatchesLegacyWindowMetadata()
    {
        var mediator = Substitute.For<IMediator>();
        var repository = Substitute.For<IReportesConsultaRepository>();
        var exporter = Substitute.For<IReporteVentasExcelExporter>();

        using var form = new frmReporteVentas(mediator, repository, exporter, 2024);

        form.ClientSize.Should().Be(new Size(804, 599));
        form.Text.Should().Be("Reporte de Ventas");
    }

    [WinFormsFact]
    public async Task BuscarAsync_WithClientFilter_SendsQueryAndEnablesExport()
    {
        var mediator = Substitute.For<IMediator>();
        var repository = Substitute.For<IReportesConsultaRepository>();
        var exporter = Substitute.For<IReporteVentasExcelExporter>();
        repository.ListarClientesAsync(Arg.Any<CancellationToken>())
            .Returns([new ClienteReporteDto(1, "Cliente Uno", "20111111111")]);
        mediator.Send(Arg.Any<ConsultarReporteVentasQuery>(), Arg.Any<CancellationToken>())
            .Returns([new VentaReporteDto(
                new DateTime(2024, 6, 10),
                1,
                "F001",
                "000001",
                "20111111111",
                "Cliente Uno",
                100m,
                18m,
                118m)]);

        using var form = new frmReporteVentas(mediator, repository, exporter, 2024);
        await form.CargarClientesAsync();
        form.SetTestCliente(1);
        form.SetTestDateRange(new DateTime(2024, 6, 1), new DateTime(2024, 6, 30));
        form.SetTestTipos(facturas: true, boletas: false);

        var success = await form.BuscarAsync();

        success.Should().BeTrue();
        form.ResultCount.Should().Be(2);
        form.FirstCliente.Should().Be("Cliente Uno");
        form.ExportEnabled.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<ConsultarReporteVentasQuery>(query =>
                query.Filtro.Year == 2024 &&
                query.Filtro.FechaDesde == new DateTime(2024, 6, 1) &&
                query.Filtro.FechaHasta == new DateTime(2024, 6, 30) &&
                query.Filtro.IncluirFacturas &&
                !query.Filtro.IncluirBoletas &&
                query.Filtro.ClienteCodigo == 1),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task ExportCurrentRows_WithSearchResults_UsesExporter()
    {
        var mediator = Substitute.For<IMediator>();
        var repository = Substitute.For<IReportesConsultaRepository>();
        var exporter = Substitute.For<IReporteVentasExcelExporter>();
        repository.ListarClientesAsync(Arg.Any<CancellationToken>())
            .Returns([new ClienteReporteDto(1, "Cliente Uno", "20111111111")]);
        mediator.Send(Arg.Any<ConsultarReporteVentasQuery>(), Arg.Any<CancellationToken>())
            .Returns([new VentaReporteDto(
                new DateTime(2024, 6, 10),
                1,
                "F001",
                "000001",
                "20111111111",
                "Cliente Uno",
                100m,
                18m,
                118m)]);
        exporter.Export(Arg.Any<IReadOnlyList<VentaReporteDto>>()).Returns([1, 2, 3]);

        using var form = new frmReporteVentas(mediator, repository, exporter, 2024);
        await form.CargarClientesAsync();
        await form.BuscarAsync();

        var bytes = form.ExportCurrentRows();

        bytes.Should().Equal(1, 2, 3);
        exporter.Received(1).Export(Arg.Any<IReadOnlyList<VentaReporteDto>>());
    }
}
