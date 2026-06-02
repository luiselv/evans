namespace EVANS.Application.Shared.DTOs;

public sealed record ParametrosDto(
    decimal IgvRate,
    string FacturaSerie,
    string FacturaNro1,
    string FacturaNro2,
    string BoletaSerie,
    string BoletaNro1,
    string BoletaNro2,
    string GuiaRemisionSerie,
    string GuiaRemisionNro1,
    string GuiaRemisionNro2,
    string Manifiesto,
    string Remitente,
    string EmailRemitente,
    string PassRemitente,
    string Smtp,
    int Puerto);
