using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Handlers;
using EVANS.Application.Reportes.Ports;
using EVANS.Application.Reportes.Queries;
using NSubstitute;

namespace EVANS.Application.Tests.Reportes;

public sealed class ConsultarEnviosMensualesQueryHandlerTests
{
    private readonly IReportesConsultaRepository _repository = Substitute.For<IReportesConsultaRepository>();

    [Fact]
    public async Task Handle_ReturnsMonthlyShipmentsFromRepository()
    {
        var filtro = new EnviosMensualesFiltro(
            Year: 2024,
            FechaDesde: new DateTime(2024, 6, 1),
            FechaHasta: new DateTime(2024, 6, 30),
            DestinoCodigos: [1, 2]);

        var expected = new List<EnvioMensualDto>
        {
            new("Cliente A", 2, new DateTime(2024, 6, 10)),
            new("Cliente B", 1, new DateTime(2024, 6, 12))
        };

        _repository.ConsultarEnviosMensualesAsync(filtro, 2024, Arg.Any<CancellationToken>())
            .Returns(expected);

        var handler = new ConsultarEnviosMensualesQueryHandler(_repository);

        var result = await handler.Handle(new ConsultarEnviosMensualesQuery(filtro), CancellationToken.None);

        result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        await _repository.Received(1)
            .ConsultarEnviosMensualesAsync(filtro, 2024, Arg.Any<CancellationToken>());
    }
}
