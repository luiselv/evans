-- Legacy compatibility bootstrap for a yearly database.
-- Run this script only with sqlcmd -d <year>; the canonical baseline is
-- db/schema-yearly.sql and hardening is versioned in db/migrations/yearly.
IF DB_NAME() = N'master'
BEGIN
    RAISERROR('Run scripts/02 transactions.sql against the target yearly database, not master.', 16, 1);
    SET NOEXEC ON;
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Manifiesto]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Manifiesto](
	[MANI_CODIGO] [int] IDENTITY(1,1) NOT NULL,
	[MANI_NUMERO] [varchar](20) NULL,
	[MANI_FECHA] [datetime] NULL,
	[EMPR_CODIGO] [int] NULL,
	[VEHI_CODIGO] [int] NULL,
	[CARR_CODIGO] [int] NULL,
	[CHOF_CODIGO] [int] NULL,
	[MANI_IMPORTE] [float] NULL,
	[MANI_NROGUIAS] [int] NULL,
	[MANI_PESO] [float] NULL,
	[ESTA_CODIGO] [int] NULL,
	[USU_CODIGO] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Recepcion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Recepcion](
	[RECE_CODIGO] [int] IDENTITY(1,1) NOT NULL,
	[RECE_FECHAEMISION] [datetime] NULL,
	[CLIE_REMITENTE] [int] NULL,
	[RECE_TIPODIRPARTIDA] [int] NULL,
	[RECE_DIRECCIONPARTIDA] [nvarchar](100) NULL,
	[CLIE_DESTINATARIO] [int] NULL,
	[RECE_TIPODIRDESTINO] [int] NULL,
	[RECE_DIRECCIONDESTINO] [nvarchar](100) NULL,
	[DEST_CODIGO] [int] NULL,
	[ESTA_CODIGO] [int] NULL,
	[RECE_BULTOS] [int] NULL,
	[RECE_PESOTOTAL] [float] NULL,
	[RECE_COSTOTOTAL] [float] NULL,
	[RECE_GUIAREMISION] [nvarchar](20) NULL,
	[RECE_OBSERVACION] [nvarchar](250) NULL,
	[USU_CODIGO] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DetalleRecepcion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DetalleRecepcion](
	[RECE_CODIGO] [int] NOT NULL,
	[DERE_CANTIDAD] [float] NULL,
	[DERE_DESCRIPCION] [nvarchar](100) NULL,
	[DERE_PESO] [float] NULL,
	[DERE_UNIDAD] [nvarchar](30) NULL,
	[DERE_COSTO] [float] NULL,
	[DERE_TIPODOC] [varchar](20) NULL,
	[DERE_NRODOC] [varchar](20) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DetalleGuia]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DetalleGuia](
	[GREM_CODIGO] [int] NOT NULL,
	[DEGR_CANTIDAD] [float] NULL,
	[DEGR_DESCRIPCION] [nvarchar](100) NULL,
	[DEGR_PESO] [float] NULL,
	[DEGR_UNIDAD] [nvarchar](30) NULL,
	[DEGR_COSTO] [float] NULL,
	[DEGR_TIPODOC] [varchar](20) NULL,
	[DEGR_NRODOC] [varchar](20) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GuiaRemision]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GuiaRemision](
	[GREM_CODIGO] [int] IDENTITY(1,1) NOT NULL,
	[GREM_SERIE] [nvarchar](4) NULL,
	[GREM_NUMERO] [nvarchar](6) NULL,
	[GREM_FECHAEMISION] [datetime] NULL,
	[GREM_FECHATRASLADO] [datetime] NULL,
	[CLIE_REMITENTE] [int] NULL,
	[GREM_TIPODIRPARTIDA] [int] NULL,
	[GREM_DIRECCIONPARTIDA] [nvarchar](100) NULL,
	[CLIE_DESTINATARIO] [int] NULL,
	[GREM_TIPODIRDESTINO] [int] NULL,
	[GREM_DIRECCIONDESTINO] [nvarchar](100) NULL,
	[DEST_CODIGO] [int] NULL,
	[VEHI_CODIGO] [int] NULL,
	[CARR_CODIGO] [int] NULL,
	[CHOF_CODIGO] [int] NULL,
	[EMPR_CODIGO] [int] NULL,
	[ESTA_CODIGO] [int] NULL,
	[GREM_BULTOS] [int] NULL,
	[GREM_PESOTOTAL] [float] NULL,
	[GREM_COSTOTOTAL] [float] NULL,
	[GREM_IMPRESO] [int] NULL,
	[TICO_CODIGO] [int] NULL,
	[GREM_DOCVENTA] [nvarchar](20) NULL,
	[GREM_OBSERVACION] [nvarchar](250) NULL,
	[USU_CODIGO] [int] NULL,
	[GREM_ENVIADO] [int] NULL,
	[GREM_MANIFIESTO] [int] NULL,
	[GREM_NROMANIFIESTO] [varchar](15) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DetalleComprobante]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DetalleComprobante](
	[COMP_CODIGO] [int] NOT NULL,
	[DECO_CANTIDAD] [float] NULL,
	[DECO_DESCRIPCION] [nvarchar](100) NULL,
	[DECO_PRECIOUNITARIO] [float] NULL,
	[DECO_FLETE] [float] NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Comprobante]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Comprobante](
	[COMP_CODIGO] [int] IDENTITY(1,1) NOT NULL,
	[COMP_SERIE] [nvarchar](4) NULL,
	[COMP_NUMERO] [nvarchar](6) NULL,
	[COMP_FECHA] [datetime] NULL,
	[CLIE_DESTINATARIO] [int] NULL,
	[COMP_DIRECCION] [nvarchar](100) NULL,
	[TICO_CODIGO] [int] NULL,
	[ESTA_CODIGO] [int] NULL,
	[COMP_GRT] [nvarchar](20) NULL,
	[CLIE_REMITENTE] [int] NULL,
	[EMPR_CODIGO] [int] NULL,
	[DEST_CODIGO] [int] NULL,
	[COMP_MANIFIESTO] [nvarchar](10) NULL,
	[COMP_VALORVENTA] [float] NULL,
	[COMP_IGV] [float] NULL,
	[COMP_TOTAL] [float] NULL,
	[COMP_IMPRESO] [int] NULL,
	[USU_CODIGO] [int] NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DetalleManifiesto]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DetalleManifiesto](
	[MANI_CODIGO] [int] NOT NULL,
	[GREM_CODIGO] [int] NOT NULL
) ON [PRIMARY]
END
GO

-- Constraints present in the 2010 production schema.
IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_GuiaRemision' AND type = 'PK')
    ALTER TABLE [dbo].[GuiaRemision] ADD CONSTRAINT PK_GuiaRemision PRIMARY KEY (GREM_CODIGO);
GO

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Comprobante' AND type = 'PK')
    ALTER TABLE [dbo].[Comprobante] ADD CONSTRAINT PK_Comprobante PRIMARY KEY (COMP_CODIGO);
GO

IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = 'PK_Manifiesto' AND type = 'PK')
    ALTER TABLE [dbo].[Manifiesto] ADD CONSTRAINT PK_Manifiesto PRIMARY KEY (MANI_CODIGO);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DETALLEGUIA_GUIAREMISION')
    ALTER TABLE [dbo].[DetalleGuia] WITH CHECK
        ADD CONSTRAINT FK_DETALLEGUIA_GUIAREMISION
        FOREIGN KEY (GREM_CODIGO) REFERENCES [dbo].[GuiaRemision](GREM_CODIGO);
GO

/* ============================== END TRANSACTIONS DATABASE SCRIPT =============================== */
