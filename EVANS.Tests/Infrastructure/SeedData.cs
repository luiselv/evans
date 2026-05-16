using Microsoft.Data.SqlClient;

namespace EVANS.Tests.Infrastructure;

public static class SeedData
{
    public static async Task InsertMinimalMasterData(SqlConnection conn)
    {
        await conn.OpenAsync();
        var cmds = new[]
        {
            "SET IDENTITY_INSERT dbo.TIPOIDENTIFICACION ON; INSERT INTO dbo.TIPOIDENTIFICACION(iden_codigo,iden_descripcion) VALUES(1,'DNI'); SET IDENTITY_INSERT dbo.TIPOIDENTIFICACION OFF",
            "SET IDENTITY_INSERT dbo.ESTADO ON; INSERT INTO dbo.ESTADO(esta_codigo,esta_descripcion) VALUES(1,'Activo'); SET IDENTITY_INSERT dbo.ESTADO OFF",
            "SET IDENTITY_INSERT dbo.TIPOCOMPROBANTE ON; INSERT INTO dbo.TIPOCOMPROBANTE(tico_codigo,tico_descripcion) VALUES(1,'Factura'),(2,'Boleta'); SET IDENTITY_INSERT dbo.TIPOCOMPROBANTE OFF",
            "INSERT INTO dbo.PARAMETROS DEFAULT VALUES",
            "INSERT INTO dbo.DESTINO(dest_nombre,dest_distanciavirtual) VALUES('Lima',0)",
            "INSERT INTO dbo.CLIENTE(clie_nombre,clie_nroidentificacion,iden_codigo) VALUES('Remitente Test','12345678',1)",
            "INSERT INTO dbo.CLIENTE(clie_nombre,clie_nroidentificacion,iden_codigo) VALUES('Destinatario Test','87654321',1)",
            "INSERT INTO dbo.EMPRESA(empr_nombre,empr_ruc,empr_direccion) VALUES('Transportista Test','20000000001','Av. Test 123')",
            "INSERT INTO dbo.VEHICULO(vehi_placa,vehi_marca,vehi_confvehicular,vehi_certinscripcion) VALUES('ABC-123','Toyota','B2','CERT-001')",
            "INSERT INTO dbo.CARRETA(carr_placa) VALUES('XYZ-999')",
            "INSERT INTO dbo.CHOFER(chof_nombre,chof_licencia,empr_codigo,esta_codigo) VALUES('Juan Chofer','LIC-001',1,1)",
            "INSERT INTO dbo.USUARIO(usu_nombrecompleto,usu_nombreusuario,usu_clave) VALUES('Admin Test','admin','1234')",
        };

        foreach (var sql in cmds)
        {
            await using var cmd = new SqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
