using EVANS.Application.Reportes.DTOs;
using EVANS.Application.Reportes.Handlers;
using EVANS.Application.Reportes.Ports;
using EVANS.Application.Reportes.Queries;
using NSubstitute;

namespace EVANS.Application.Tests.Reportes;

public sealed class ConsultarGuiasPorClienteQueryHandlerTests
{
    [Fact]
    public async Task Handle_DelegatesToRepositoryUsingFilterYear()
    {
        var repository = Substitute.For<IReportesConsultaRepository>();
        var expected = new List<GuiaPorClienteDto>
        {
            new(
                10,
                "GR01-000010",
                "Cliente Uno",
                "Cliente Dos",
                new DateTime(2024, 6, 10),
                new DateTime(2024, 6, 11),
                2,
                100m,
                false)
        };
        repository.ConsultarGuiasPorClienteAsync(
                Arg.Any<GuiasPorClienteFiltro>(),
                Arg.Any<int>(),
                Arg.Any<CancellationToken>())
            .Returns(expected);

        var handler = new ConsultarGuiasPorClienteQueryHandler(repository);
        var filtro = new GuiasPorClienteFiltro(
            2024,
            1,
            new DateTime(2024, 6, 1),
            new DateTime(2024, 6, 30),
            SoloPendientes: true);

        var result = await handler.Handle(new ConsultarGuiasPorClienteQuery(filtro), CancellationToken.None);

        result.Should().BeEquivalentTo(expected);
        await repository.Received(1).ConsultarGuiasPorClienteAsync(
            filtro,
            2024,
            Arg.Any<CancellationToken>());
    }
}
