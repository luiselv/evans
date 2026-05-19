using EVANS.Application.GuiaRemision.Commands;
using EVANS.Application.GuiaRemision.Ports;
using EVANS.Domain.GuiaRemision;
using NSubstitute;

namespace EVANS.Application.Tests.GuiaRemision;

public class EliminarGuiaCommandHandlerTests
{
    private readonly IGuiaRepository _repo;
    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly IUnitOfWork _uow;
    private readonly EliminarGuiaCommandHandler _handler;

    public EliminarGuiaCommandHandlerTests()
    {
        _repo = Substitute.For<IGuiaRepository>();
        _uowFactory = Substitute.For<IUnitOfWorkFactory>();
        _uow = Substitute.For<IUnitOfWork>();

        _uowFactory.Create(Arg.Any<int>()).Returns(_uow);

        _handler = new EliminarGuiaCommandHandler(_repo, _uowFactory);
    }

    [Fact]
    public async Task Handle_Valid_RepoEliminarCalledWithUow()
    {
        // Arrange
        var command = new EliminarGuiaCommand(Codigo: 7, Year: 2024);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repo.Received(1).Eliminar(7, _uow);
    }

    [Fact]
    public async Task Handle_MissingGuide_IdempotentNoException()
    {
        // Arrange — repo does nothing (already default behaviour for void method)
        var command = new EliminarGuiaCommand(Codigo: 99, Year: 2024);

        // Act — must not throw
        var exception = await Record.ExceptionAsync(() =>
            _handler.Handle(command, CancellationToken.None));

        // Assert
        exception.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Valid_CommitCalled()
    {
        // Arrange
        var command = new EliminarGuiaCommand(Codigo: 7, Year: 2024);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _uow.Received(1).Commit();
    }
}
