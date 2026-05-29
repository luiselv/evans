namespace EVANS.Domain.Shared;

public record Direccion(string Calle, string Ciudad, string Provincia)
{
    public static readonly Direccion Empty = new("", "", "");

    public static Direccion Parse(string raw)
    {
        if (string.IsNullOrEmpty(raw))
            return Empty;

        var parts = raw.Split('|');

        var calle = parts.Length > 0 ? parts[0] : "";
        var ciudad = parts.Length > 1 ? parts[1] : "";
        var provincia = parts.Length > 2 ? parts[2] : "";

        return new Direccion(calle, ciudad, provincia);
    }

    public string Serialize() => $"{Calle}|{Ciudad}|{Provincia}";
}
