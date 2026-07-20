namespace EVANS.Tests;

public sealed class YearlySchemaParityTests
{
    [Fact]
    public void Baseline_SeparatesProductionSchemaFromHardening()
    {
        var baseline = File.ReadAllText(DbPath("schema-yearly.sql"));

        Assert.Contains("PK_GuiaRemision", baseline);
        Assert.Contains("PK_Comprobante", baseline);
        Assert.Contains("PK_Manifiesto", baseline);
        Assert.Contains("FK_DETALLEGUIA_GUIAREMISION", baseline);
        Assert.DoesNotContain("PK_Recepcion", baseline);
        Assert.DoesNotContain("CREATE NONCLUSTERED INDEX", baseline);
    }

    [Fact]
    public void HardeningMigrations_AreExplicitAndSeparated()
    {
        var indexes = File.ReadAllText(DbPath("migrations", "yearly", "V001__add_yearly_performance_indexes.sql"));
        var recepcionPrimaryKey = File.ReadAllText(DbPath("migrations", "yearly", "V002__add_recepcion_primary_key.sql"));

        Assert.Contains("IX_GuiaRemision_SerieNumero", indexes);
        Assert.DoesNotContain("PK_Recepcion", indexes);
        Assert.Contains("PK_Recepcion", recepcionPrimaryKey);
        Assert.Contains("duplicate values", recepcionPrimaryKey);
    }

    [Fact]
    public void LegacyBootstrap_RequiresAnExplicitTargetDatabase()
    {
        var bootstrap = File.ReadAllText(Path.Combine(RepositoryRoot(), "scripts", "02 transactions.sql"));

        Assert.Contains("DB_NAME() = N'master'", bootstrap);
        Assert.DoesNotContain("CREATE DATABASE", bootstrap);
        Assert.Contains("PK_GuiaRemision", bootstrap);
        Assert.Contains("FK_DETALLEGUIA_GUIAREMISION", bootstrap);
    }

    private static string DbPath(params string[] pathSegments)
        => Path.Combine([RepositoryRoot(), "db", .. pathSegments]);

    private static string RepositoryRoot()
    {
        var directory = AppContext.BaseDirectory;
        while (directory != null && !Directory.Exists(Path.Combine(directory, "db")))
            directory = Directory.GetParent(directory)?.FullName;

        return directory!;
    }
}
