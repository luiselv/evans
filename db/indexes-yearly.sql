-- Additive hardening for existing yearly transactional databases.
-- Safe to run on any yearly DB (2010, 2014, 2019, 2026 ...) without touching data.
-- Idempotent: each statement is guarded by IF NOT EXISTS.
-- Run once per existing DB during a maintenance window.

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_GuiaRemision' AND type = 'PK')
    ALTER TABLE GuiaRemision ADD CONSTRAINT PK_GuiaRemision PRIMARY KEY (GREM_CODIGO);
GO

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Comprobante' AND type = 'PK')
    ALTER TABLE Comprobante ADD CONSTRAINT PK_Comprobante PRIMARY KEY (COMP_CODIGO);
GO

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Manifiesto' AND type = 'PK')
    ALTER TABLE Manifiesto ADD CONSTRAINT PK_Manifiesto PRIMARY KEY (MANI_CODIGO);
GO

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Recepcion' AND type = 'PK')
    ALTER TABLE Recepcion ADD CONSTRAINT PK_Recepcion PRIMARY KEY (RECE_CODIGO);
GO

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
