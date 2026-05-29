namespace EVANS.Domain.Manifiesto;

public record GuiasMarcadoResult(int Affected, IReadOnlyList<int> NotFound)
{
    public bool HasFailures => NotFound.Count > 0;
}
