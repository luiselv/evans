namespace EVANS.Domain.Manifiesto;

public class DomainException : Exception
{
    public string Code { get; }

    public DomainException(string code, string message) : base(message)
    {
        Code = code;
    }

    // Keep single-arg overload for compatibility
    public DomainException(string message) : base(message)
    {
        Code = "DOMAIN_ERROR";
    }
}
