using System.Reflection;
using EVANS.Application.Catalogo.Ports;
using EVANS.Domain.Catalogo;

namespace EVANS.Application.Tests.Catalogo;

public class CatalogoPortContractTests
{
    [Theory]
    [InlineData(typeof(Empresa))]
    [InlineData(typeof(Vehiculo))]
    [InlineData(typeof(Carreta))]
    [InlineData(typeof(Chofer))]
    [InlineData(typeof(Destino))]
    public void Repository_ForStatusBackedEntities_ExposesDeactivateButNoDelete(Type entityType)
    {
        var repositoryType = typeof(IRepository<>).MakeGenericType(entityType);

        repositoryType.GetMethod("DeactivateAsync").Should().NotBeNull();
        repositoryType.GetMethod("DeleteAsync").Should().BeNull();
    }

    [Theory]
    [InlineData(typeof(IClienteRepository))]
    [InlineData(typeof(IEstadoRepository))]
    [InlineData(typeof(ITipoIdentificacionRepository))]
    public void NonStatusBackedRepositories_DoNotExposeDeactivateOrDelete(Type repositoryType)
    {
        repositoryType.GetMethod("DeactivateAsync").Should().BeNull();
        repositoryType.GetMethod("DeleteAsync").Should().BeNull();
    }

    [Fact]
    public void AgenciaRepository_IsReadOnly()
    {
        var methods = typeof(IAgenciaRepository)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Select(method => method.Name)
            .ToArray();

        methods.Should().BeEquivalentTo("GetByIdAsync", "ListActiveAsync");
    }

    [Fact]
    public void ClienteRepository_ExposesCreateAndUpdateButNoDeactivate()
    {
        var methods = typeof(IClienteRepository)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Select(method => method.Name)
            .ToArray();

        methods.Should().Contain(["GetByIdAsync", "ListAsync", "AddAsync", "UpdateAsync"]);
        methods.Should().NotContain(["DeactivateAsync", "DeleteAsync"]);
    }

}
