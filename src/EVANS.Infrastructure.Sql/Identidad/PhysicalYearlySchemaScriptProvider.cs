namespace EVANS.Infrastructure.Sql.Identidad;

public sealed class PhysicalYearlySchemaScriptProvider : IYearlySchemaScriptProvider
{
    public async Task<string> ReadSchemaAsync(CancellationToken cancellationToken = default)
    {
        var path = FindSchemaFile();
        return await File.ReadAllTextAsync(path, cancellationToken);
    }

    private static string FindSchemaFile()
    {
        var baseDir = new DirectoryInfo(AppContext.BaseDirectory);
        for (var dir = baseDir; dir is not null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, "db", "schema-yearly.sql");
            if (File.Exists(candidate))
                return candidate;
        }

        throw new FileNotFoundException("Yearly schema file 'db/schema-yearly.sql' was not found.");
    }
}
