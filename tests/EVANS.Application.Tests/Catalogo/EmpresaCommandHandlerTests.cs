using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;
using EVANS.Domain.Shared;
using NSubstitute;

namespace EVANS.Application.Tests.Catalogo;

public sealed class EmpresaCommandHandlerTests
{
    [Fact]
    public async Task CreateEmpresaCommandHandler_ValidCommand_AddsEmpresaAndReturnsCodigo()
    {
        var repo = Substitute.For<IRepository<Empresa>>();
        repo.AddAsync(Arg.Any<Empresa>(), Arg.Any<CancellationToken>())
            .Returns(7);

        var handler = new CreateEmpresaCommandHandler(repo);

        var result = await handler.Handle(
            new CreateEmpresaCommand("TRANSPORT SA", "Av Lima", "555", "20123456789", true),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(7);
        await repo.Received(1).AddAsync(
            Arg.Is<Empresa>(empresa =>
                empresa.RazonSocial == "TRANSPORT SA" &&
                empresa.Ruc.Value == "20123456789" &&
                empresa.EstadoCodigo == CatalogoEstado.Activo),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateEmpresaCommandHandler_InvalidRuc_ReturnsFailureWithoutRepositoryCall()
    {
        var repo = Substitute.For<IRepository<Empresa>>();
        var handler = new CreateEmpresaCommandHandler(repo);

        var result = await handler.Handle(
            new CreateEmpresaCommand("TRANSPORT SA", null, null, "invalid", true),
            CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        await repo.DidNotReceive().AddAsync(Arg.Any<Empresa>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateEmpresaCommandHandler_ExistingEmpresa_UpdatesRepository()
    {
        Ruc.TryCreate("20123456789", out var originalRuc).Should().BeTrue();
        var repo = Substitute.For<IRepository<Empresa>>();
        repo.GetByIdAsync(7, Arg.Any<CancellationToken>())
            .Returns(Empresa.Materializar(7, "OLD", null, null, originalRuc, false, CatalogoEstado.Activo));

        var handler = new UpdateEmpresaCommandHandler(repo);

        var result = await handler.Handle(
            new UpdateEmpresaCommand(7, "TRANSPORT SA", "Av Lima", "555", "20987654321", true),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await repo.Received(1).UpdateAsync(
            Arg.Is<Empresa>(empresa =>
                empresa.Codigo == 7 &&
                empresa.RazonSocial == "TRANSPORT SA" &&
                empresa.Ruc.Value == "20987654321" &&
                empresa.EsPropia),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateEmpresaCommandHandler_MissingEmpresa_ReturnsFailureWithoutUpdate()
    {
        var repo = Substitute.For<IRepository<Empresa>>();
        repo.GetByIdAsync(7, Arg.Any<CancellationToken>()).Returns((Empresa?)null);

        var handler = new UpdateEmpresaCommandHandler(repo);

        var result = await handler.Handle(
            new UpdateEmpresaCommand(7, "TRANSPORT SA", null, null, "20123456789", true),
            CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        await repo.DidNotReceive().UpdateAsync(Arg.Any<Empresa>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeactivateEmpresaCommandHandler_ExistingEmpresa_DeactivatesRepository()
    {
        Ruc.TryCreate("20123456789", out var ruc).Should().BeTrue();
        var repo = Substitute.For<IRepository<Empresa>>();
        repo.GetByIdAsync(7, Arg.Any<CancellationToken>())
            .Returns(Empresa.Materializar(7, "TRANSPORT SA", null, null, ruc, true, CatalogoEstado.Activo));

        var handler = new DeactivateEmpresaCommandHandler(repo);

        var result = await handler.Handle(new DeactivateEmpresaCommand(7), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await repo.Received(1).DeactivateAsync(7, Arg.Any<CancellationToken>());
    }
}
