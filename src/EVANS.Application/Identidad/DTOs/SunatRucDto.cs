namespace EVANS.Application.Identidad.DTOs;

public sealed record SunatRucDto(
    string Ruc,
    string RazonSocial,
    string Estado,
    string Condicion,
    string Direccion);
