namespace EVANS.Domain.Recepcion;

/// <summary>
/// Domain exception for Recepcion bounded context.
/// Carries a machine-readable code alongside the human-readable message.
/// </summary>
public sealed class DomainException : Exception
{
    public string Code { get; }

    public DomainException(string code, string message) : base(message)
    {
        Code = code;
    }
}
