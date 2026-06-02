using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Handlers;
using EVANS.Application.Reportes.Ports;
using EVANS.Application.Reportes.Queries;
using NSubstitute;

namespace EVANS.Application.Tests.Reportes;

public sealed class ConsultarReporteVentasQueryHandlerTests
{
    [Fact]
    public async Task Handle_DelegatesToRepositoryUsingFilterYear()
    {
        var repository = Substitute.For<IReportesConsultaRepository>();
        var expected = new List<VentaReporteDto>
        {
            new(
                new DateTime(2024, 6, 10),
                1,
                "F001",
                "000001",
                "20111111111",
                "Cliente Uno",
                100m,
                18m,
                118m)
        };
        repository.ConsultarReporteVentasAsync(
                Arg.Any<ReporteVentasFiltro>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Returns(expected);

        var handler = new ConsultarReporteVentasQueryHandler(repository);
        var filtro = new ReporteVentasFiltro(
            2024,
            new DateTime(2024, 6, 1),
            new DateTime(2024, 6, 30),
            IncluirFacturas: true,
            IncluirBoletas: false,
            ClienteCodigo: 1);

        var result = await handler.Handle(new ConsultarReporteVentasQuery(filtro), CancellationToken.None);

        result.Should().BeEquivalentTo(expected);
        await repository.Received(1).ConsultarReporteVentasAsync(
            filtro,
            2024,
            Arg.Any<CancellationToken>());
    }
}
