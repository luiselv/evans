-- Yearly transactional database schema
-- Derived from VB.NET business classes (clsGuiaRemision, clsComprobante, clsManifiesto, clsRecepcion)
-- Mirrors structure created by CrearBD() in modMetodos.vb

CREATE TABLE GuiaRemision (
    grem_codigo         INT           IDENTITY(1,1) PRIMARY KEY,
    grem_serie          NVARCHAR(4)   NOT NULL,
    grem_numero         NVARCHAR(6)   NOT NULL,
    grem_fechaemision   DATETIME      NOT NULL,
    grem_fechatraslado  DATETIME      NOT NULL,
    clie_remitente      INT           NOT NULL,
    grem_tipodirpartida INT           NOT NULL,
    grem_direccionpartida NVARCHAR(100) NOT NULL,
    clie_destinatario   INT           NOT NULL,
    grem_tipodirdestino INT           NOT NULL,
    grem_direcciondestino NVARCHAR(100) NOT NULL,
    dest_codigo         INT           NOT NULL,
    vehi_codigo         INT           NOT NULL,
    carr_codigo         INT           NOT NULL,
    chof_codigo         INT           NOT NULL,
    empr_codigo         INT           NOT NULL,
    esta_codigo         INT           NOT NULL,
    grem_bultos         INT           NOT NULL DEFAULT 0,
    grem_pesototal      FLOAT         NOT NULL DEFAULT 0,
    grem_costototal     FLOAT         NOT NULL DEFAULT 0,
    grem_impreso        INT           NOT NULL DEFAULT 0,
    tico_codigo         INT           NOT NULL,
    grem_docventa       NVARCHAR(20)  NULL,
    grem_observacion    NVARCHAR(250) NULL,
    usu_codigo          INT           NOT NULL,
    grem_enviado        INT           NOT NULL DEFAULT 0,
    grem_manifiesto     INT           NOT NULL DEFAULT 0,
    grem_nromanifiesto  NVARCHAR(20)  NULL
);
GO

CREATE TABLE DetalleGuia (
    grem_codigo         INT           NOT NULL,
    degr_cantidad       FLOAT         NOT NULL DEFAULT 0,
    degr_descripcion    NVARCHAR(100) NOT NULL,
    degr_peso           FLOAT         NOT NULL DEFAULT 0,
    degr_unidad         NVARCHAR(30)  NULL,
    degr_costo          FLOAT         NOT NULL DEFAULT 0,
    degr_tipodoc        VARCHAR(20)   NULL,
    degr_nrodoc         VARCHAR(20)   NULL
);
GO

CREATE TABLE Comprobante (
    comp_codigo         INT           IDENTITY(1,1) PRIMARY KEY,
    comp_serie          NVARCHAR(4)   NOT NULL,
    comp_numero         NVARCHAR(6)   NOT NULL,
    comp_fecha          DATETIME      NOT NULL,
    clie_destinatario   INT           NOT NULL,
    comp_direccion      NVARCHAR(100) NULL,
    tico_codigo         INT           NOT NULL,
    esta_codigo         INT           NOT NULL,
    comp_grt            NVARCHAR(20)  NULL,
    clie_remitente      INT           NOT NULL,
    empr_codigo         INT           NOT NULL,
    dest_codigo         INT           NOT NULL,
    comp_manifiesto     NVARCHAR(10)  NULL,
    comp_valorventa     FLOAT         NOT NULL DEFAULT 0,
    comp_igv            FLOAT         NOT NULL DEFAULT 0,
    comp_total          FLOAT         NOT NULL DEFAULT 0,
    comp_impreso        INT           NOT NULL DEFAULT 0,
    usu_codigo          INT           NOT NULL
);
GO

CREATE TABLE DetalleComprobante (
    comp_codigo         INT           NOT NULL,
    deco_cantidad       FLOAT         NOT NULL DEFAULT 0,
    deco_descripcion    NVARCHAR(100) NOT NULL,
    deco_preciounitario FLOAT         NOT NULL DEFAULT 0,
    deco_flete          FLOAT         NOT NULL DEFAULT 0
);
GO

CREATE TABLE Manifiesto (
    mani_codigo         INT           IDENTITY(1,1) PRIMARY KEY,
    mani_numero         VARCHAR(20)   NOT NULL,
    mani_fecha          DATETIME      NOT NULL,
    empr_codigo         INT           NOT NULL,
    vehi_codigo         INT           NOT NULL,
    carr_codigo         INT           NOT NULL,
    chof_codigo         INT           NOT NULL,
    mani_importe        FLOAT         NOT NULL DEFAULT 0,
    mani_nroguias       INT           NOT NULL DEFAULT 0,
    mani_peso           FLOAT         NOT NULL DEFAULT 0,
    esta_codigo         INT           NOT NULL,
    usu_codigo          INT           NOT NULL
);
GO

CREATE TABLE DetalleManifiesto (
    mani_codigo         INT           NOT NULL,
    grem_codigo         INT           NOT NULL
);
GO

CREATE TABLE Recepcion (
    rece_codigo         INT           IDENTITY(1,1) PRIMARY KEY,
    rece_fechaemision   DATETIME      NOT NULL,
    clie_remitente      INT           NOT NULL,
    rece_tipodirpartida INT           NOT NULL,
    rece_direccionpartida NVARCHAR(100) NOT NULL,
    clie_destinatario   INT           NOT NULL,
    rece_tipodirdestino INT           NOT NULL,
    rece_direcciondestino NVARCHAR(100) NOT NULL,
    dest_codigo         INT           NOT NULL,
    esta_codigo         INT           NOT NULL,
    rece_bultos         INT           NOT NULL DEFAULT 0,
    rece_pesototal      FLOAT         NOT NULL DEFAULT 0,
    rece_costototal     FLOAT         NOT NULL DEFAULT 0,
    rece_guiaremision   NVARCHAR(20)  NULL,
    rece_observacion    NVARCHAR(250) NULL,
    usu_codigo          INT           NOT NULL
);
GO

CREATE TABLE DetalleRecepcion (
    rece_codigo         INT           NOT NULL,
    dere_cantidad       FLOAT         NOT NULL DEFAULT 0,
    dere_descripcion    NVARCHAR(100) NOT NULL,
    dere_peso           FLOAT         NOT NULL DEFAULT 0,
    dere_unidad         NVARCHAR(30)  NULL,
    dere_costo          FLOAT         NOT NULL DEFAULT 0,
    dere_tipodoc        VARCHAR(20)   NULL,
    dere_nrodoc         VARCHAR(20)   NULL
);
GO
