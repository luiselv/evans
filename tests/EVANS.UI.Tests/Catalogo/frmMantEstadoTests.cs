using EVANS.Application.Catalogo.Commands;
using EVANS.Application.Catalogo.DTOs;
using EVANS.Application.Catalogo.Queries;
using EVANS.Application.Common;
using EVANS.UI.WinForms.Catalogo;
using MediatR;

namespace EVANS.UI.Tests.Catalogo;

public sealed class FrmMantEstadoTests
{
    [WinFormsFact]
    public void Constructor_MatchesLegacyLayoutContract()
    {
        var mediator = Substitute.For<IMediator>();

        using var form = new frmMantEstado(mediator);

        form.Text.Should().Be("Registro de Estados");
        form.ClientSize.Should().Be(new Size(635, 490));

        var btnNuevo = form.Controls.Find("btnNuevo", searchAllChildren: true).Single();
        btnNuevo.Location.Should().Be(new Point(561, 82));
        btnNuevo.Size.Should().Be(new Size(62, 48));
        btnNuevo.Text.Should().Be("Nuevo");

        var tabControl = form.Controls.Find("TabControl1", searchAllChildren: true).Single();
        tabControl.Location.Should().Be(new Point(12, 12));
        tabControl.Size.Should().Be(new Size(537, 458));

        var txtDescripcion = form.Controls.Find("txtDescripcion", searchAllChildren: true).Single();
        txtDescripcion.Location.Should().Be(new Point(201, 84));
        txtDescripcion.Size.Width.Should().Be(145);
    }

    [WinFormsFact]
    public void Constructor_MatchesLegacyInitialState()
    {
        var mediator = Substitute.For<IMediator>();

        using var form = new frmMantEstado(mediator);

        form.SelectedTabIndex.Should().Be(0);
        form.ListingTabEnabled.Should().BeTrue();
        form.DetailsTabEnabled.Should().BeFalse();
        form.BuscarTextEnabled.Should().BeTrue();
        form.BuscarEnabled.Should().BeTrue();
        form.NuevoEnabled.Should().BeTrue();
        form.GrabarEnabled.Should().BeFalse();
        form.EditarEnabled.Should().BeFalse();
        form.CancelarEnabled.Should().BeFalse();
        form.GrabarText.Should().Be("Grabar");
    }

    [WinFormsFact]
    public async Task LoadEstadosAsync_DisplaysLegacyListColumnsData()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ListEstadosQuery>(), Arg.Any<CancellationToken>())
            .Returns([new EstadoDto(1, "ACTIVO"), new EstadoDto(2, "INACTIVO")]);

        using var form = new frmMantEstado(mediator);

        await form.LoadEstadosAsync();

        form.ListCount.Should().Be(2);
        form.FirstListDescription.Should().Be("ACTIVO");
    }

    [WinFormsFact]
    public async Task BuscarAsync_EmptySearch_ShowsLegacyInsufficientDataMessage()
    {
        var mediator = Substitute.For<IMediator>();
        using var form = new frmMantEstado(mediator);
        form.SetTestSearchText("");

        var success = await form.BuscarAsync();

        success.Should().BeFalse();
        form.StatusMessage.Should().Be("Ingrese nombre a buscar");
        await mediator.DidNotReceive().Send(Arg.Any<ListEstadosQuery>(), Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task BuscarAsync_FiltersByNamePrefix()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ListEstadosQuery>(), Arg.Any<CancellationToken>())
            .Returns([new EstadoDto(1, "ACTIVO"), new EstadoDto(2, "INACTIVO")]);

        using var form = new frmMantEstado(mediator);
        form.SetTestSearchText("ACT");

        var success = await form.BuscarAsync();

        success.Should().BeTrue();
        form.ListCount.Should().Be(1);
        form.FirstListDescription.Should().Be("ACTIVO");
    }

    [WinFormsFact]
    public async Task SaveAsync_NewEstado_UppercasesDescriptionAndReturnsToListing()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<CreateEstadoCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<int>.Ok(5));
        mediator.Send(Arg.Any<ListEstadosQuery>(), Arg.Any<CancellationToken>())
            .Returns(Array.Empty<EstadoDto>());

        using var form = new frmMantEstado(mediator);
        form.BeginNewForTest();
        form.SetTestDescripcion("activo");

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        form.SelectedTabIndex.Should().Be(0);
        form.DetailsTabEnabled.Should().BeFalse();
        form.GrabarEnabled.Should().BeFalse();
        await mediator.Received(1).Send(
            Arg.Is<CreateEstadoCommand>(command => command.Descripcion == "ACTIVO"),
            Arg.Any<CancellationToken>());
    }

    [WinFormsFact]
    public async Task OpenAndEditEstado_UsesGetByIdThenUpdateCommand()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<ListEstadosQuery>(), Arg.Any<CancellationToken>())
            .Returns([new EstadoDto(2, "INACTIVO")]);
        mediator.Send(Arg.Any<GetEstadoByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new EstadoDto(2, "INACTIVO"));
        mediator.Send(Arg.Any<UpdateEstadoCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<bool>.Ok(true));

        using var form = new frmMantEstado(mediator);
        await form.LoadEstadosAsync();

        await form.OpenFirstForTestAsync();
        form.SelectedTabIndex.Should().Be(1);
        form.CodigoText.Should().Be("2");
        form.EditarEnabled.Should().BeTrue();

        form.BeginEditForTest();
        form.GrabarText.Should().Be("Actualizar");
        form.SetTestDescripcion("cerrado");

        var success = await form.SaveAsync();

        success.Should().BeTrue();
        await mediator.Received(1).Send(
            Arg.Is<UpdateEstadoCommand>(command => command.Codigo == 2 && command.Descripcion == "CERRADO"),
            Arg.Any<CancellationToken>());
    }
}
