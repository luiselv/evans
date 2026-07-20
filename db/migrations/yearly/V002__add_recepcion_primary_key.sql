-- Optional additive hardening for yearly transactional databases.
-- Do not apply until the target database passes these data-integrity checks.

IF NOT EXISTS (
    SELECT 1
    FROM sys.key_constraints
    WHERE parent_object_id = OBJECT_ID(N'dbo.Recepcion')
      AND type = 'PK')
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.Recepcion WHERE RECE_CODIGO IS NULL)
    BEGIN
        RAISERROR('Cannot add PK_Recepcion: RECE_CODIGO contains NULL values.', 16, 1);
        RETURN;
    END;

    IF EXISTS (
        SELECT RECE_CODIGO
        FROM dbo.Recepcion
        GROUP BY RECE_CODIGO
        HAVING COUNT(*) > 1)
    BEGIN
        RAISERROR('Cannot add PK_Recepcion: RECE_CODIGO contains duplicate values.', 16, 1);
        RETURN;
    END;

    ALTER TABLE dbo.Recepcion
        ADD CONSTRAINT PK_Recepcion PRIMARY KEY (RECE_CODIGO);
END;
GO
