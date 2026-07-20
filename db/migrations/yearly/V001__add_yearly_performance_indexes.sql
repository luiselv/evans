-- Optional additive hardening for yearly transactional databases.
-- Run during a maintenance window after the production-faithful baseline exists.

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DetalleGuia_GREM')
    CREATE NONCLUSTERED INDEX IX_DetalleGuia_GREM ON DetalleGuia(GREM_CODIGO);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DetalleComprobante_COMP')
    CREATE NONCLUSTERED INDEX IX_DetalleComprobante_COMP ON DetalleComprobante(COMP_CODIGO);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DetalleManifiesto_MANI')
    CREATE NONCLUSTERED INDEX IX_DetalleManifiesto_MANI ON DetalleManifiesto(MANI_CODIGO);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DetalleManifiesto_GREM')
    CREATE NONCLUSTERED INDEX IX_DetalleManifiesto_GREM ON DetalleManifiesto(GREM_CODIGO);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_DetalleRecepcion_RECE')
    CREATE NONCLUSTERED INDEX IX_DetalleRecepcion_RECE ON DetalleRecepcion(RECE_CODIGO);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_GuiaRemision_SerieNumero')
    CREATE NONCLUSTERED INDEX IX_GuiaRemision_SerieNumero ON GuiaRemision(GREM_SERIE, GREM_NUMERO);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_GuiaRemision_Fecha')
    CREATE NONCLUSTERED INDEX IX_GuiaRemision_Fecha ON GuiaRemision(GREM_FECHAEMISION);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Comprobante_SerieNumero')
    CREATE NONCLUSTERED INDEX IX_Comprobante_SerieNumero ON Comprobante(COMP_SERIE, COMP_NUMERO);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Comprobante_Fecha')
    CREATE NONCLUSTERED INDEX IX_Comprobante_Fecha ON Comprobante(COMP_FECHA);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Manifiesto_Fecha')
    CREATE NONCLUSTERED INDEX IX_Manifiesto_Fecha ON Manifiesto(MANI_FECHA);
GO
