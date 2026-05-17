-- Yearly transactional database schema
-- Derived from production SQL Server DB (attached as "2014", copy of 2010).
-- Real DB has no PKs, no FKs, no defaults, no CHECK constraints.
-- Recepcion/DetalleRecepcion not present in pre-2014 DBs but exist in clsRecepcion.vb.

CREATE TABLE GuiaRemision (
    GREM_CODIGO          INT           IDENTITY(1,1) NOT NULL,
    GREM_SERIE           NVARCHAR(4)   NULL,
    GREM_NUMERO          NVARCHAR(6)   NULL,
    GREM_FECHAEMISION    DATETIME      NULL,
    GREM_FECHATRASLADO   DATETIME      NULL,
    CLIE_REMITENTE       INT           NULL,
    GREM_TIPODIRPARTIDA  INT           NULL,
    GREM_DIRECCIONPARTIDA NVARCHAR(100) NULL,
    CLIE_DESTINATARIO    INT           NULL,
    GREM_TIPODIRDESTINO  INT           NULL,
    GREM_DIRECCIONDESTINO NVARCHAR(100) NULL,
    DEST_CODIGO          INT           NULL,
    VEHI_CODIGO          INT           NULL,
    CARR_CODIGO          INT           NULL,
    CHOF_CODIGO          INT           NULL,
    EMPR_CODIGO          INT           NULL,
    ESTA_CODIGO          INT           NULL,
    GREM_BULTOS          INT           NULL,
    GREM_PESOTOTAL       FLOAT         NULL,
    GREM_COSTOTOTAL      FLOAT         NULL,
    GREM_IMPRESO         INT           NULL,
    TICO_CODIGO          INT           NULL,
    GREM_DOCVENTA        NVARCHAR(20)  NULL,
    GREM_OBSERVACION     NVARCHAR(250) NULL,
    USU_CODIGO           INT           NULL,
    GREM_ENVIADO         INT           NULL,
    GREM_MANIFIESTO      INT           NULL,
    GREM_NROMANIFIESTO   VARCHAR(15)   NULL
);
GO

CREATE TABLE DetalleGuia (
    GREM_CODIGO          INT           NOT NULL,
    DEGR_CANTIDAD        FLOAT         NULL,
    DEGR_DESCRIPCION     NVARCHAR(100) NULL,
    DEGR_PESO            FLOAT         NULL,
    DEGR_UNIDAD          NVARCHAR(30)  NULL,
    DEGR_COSTO           FLOAT         NULL,
    DEGR_TIPODOC         VARCHAR(20)   NULL,
    DEGR_NRODOC          VARCHAR(20)   NULL
);
GO

CREATE TABLE Comprobante (
    COMP_CODIGO          INT           IDENTITY(1,1) NOT NULL,
    COMP_SERIE           NVARCHAR(4)   NULL,
    COMP_NUMERO          NVARCHAR(6)   NULL,
    COMP_FECHA           DATETIME      NULL,
    CLIE_DESTINATARIO    INT           NULL,
    COMP_DIRECCION       NVARCHAR(100) NULL,
    TICO_CODIGO          INT           NULL,
    ESTA_CODIGO          INT           NULL,
    COMP_GRT             NVARCHAR(20)  NULL,
    CLIE_REMITENTE       INT           NULL,
    EMPR_CODIGO          INT           NULL,
    DEST_CODIGO          INT           NULL,
    COMP_MANIFIESTO      NVARCHAR(10)  NULL,
    COMP_VALORVENTA      FLOAT         NULL,
    COMP_IGV             FLOAT         NULL,
    COMP_TOTAL           FLOAT         NULL,
    COMP_IMPRESO         INT           NULL,
    USU_CODIGO           INT           NULL
);
GO

CREATE TABLE DetalleComprobante (
    COMP_CODIGO          INT           NOT NULL,
    DECO_CANTIDAD        FLOAT         NULL,
    DECO_DESCRIPCION     NVARCHAR(100) NULL,
    DECO_PRECIOUNITARIO  FLOAT         NULL,
    DECO_FLETE           FLOAT         NULL
);
GO

CREATE TABLE Manifiesto (
    MANI_CODIGO          INT           IDENTITY(1,1) NOT NULL,
    MANI_NUMERO          VARCHAR(20)   NULL,
    MANI_FECHA           DATETIME      NULL,
    EMPR_CODIGO          INT           NULL,
    VEHI_CODIGO          INT           NULL,
    CARR_CODIGO          INT           NULL,
    CHOF_CODIGO          INT           NULL,
    MANI_IMPORTE         FLOAT         NULL,
    MANI_NROGUIAS        INT           NULL,
    MANI_PESO            FLOAT         NULL,
    ESTA_CODIGO          INT           NULL,
    USU_CODIGO           INT           NULL
);
GO

CREATE TABLE DetalleManifiesto (
    MANI_CODIGO          INT           NOT NULL,
    GREM_CODIGO          INT           NOT NULL
);
GO

-- Recepcion and DetalleRecepcion not present in pre-2014 yearly DBs.
-- Included here because clsRecepcion.vb exists in the VB.NET source.
CREATE TABLE Recepcion (
    RECE_CODIGO           INT           IDENTITY(1,1) NOT NULL,
    RECE_FECHAEMISION     DATETIME      NULL,
    CLIE_REMITENTE        INT           NULL,
    RECE_TIPODIRPARTIDA   INT           NULL,
    RECE_DIRECCIONPARTIDA NVARCHAR(100) NULL,
    CLIE_DESTINATARIO     INT           NULL,
    RECE_TIPODIRDESTINO   INT           NULL,
    RECE_DIRECCIONDESTINO NVARCHAR(100) NULL,
    DEST_CODIGO           INT           NULL,
    ESTA_CODIGO           INT           NULL,
    RECE_BULTOS           INT           NULL,
    RECE_PESOTOTAL        FLOAT         NULL,
    RECE_COSTOTOTAL       FLOAT         NULL,
    RECE_GUIAREMISION     NVARCHAR(20)  NULL,
    RECE_OBSERVACION      NVARCHAR(250) NULL,
    USU_CODIGO            INT           NULL
);
GO

CREATE TABLE DetalleRecepcion (
    RECE_CODIGO          INT           NOT NULL,
    DERE_CANTIDAD        FLOAT         NULL,
    DERE_DESCRIPCION     NVARCHAR(100) NULL,
    DERE_PESO            FLOAT         NULL,
    DERE_UNIDAD          NVARCHAR(30)  NULL,
    DERE_COSTO           FLOAT         NULL,
    DERE_TIPODOC         VARCHAR(20)   NULL,
    DERE_NRODOC          VARCHAR(20)   NULL
);
GO
