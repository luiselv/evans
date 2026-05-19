namespace EVANS.Host.WinForms.Shell;

/// <summary>
/// Static feature flags that gate new-stack forms versus legacy code paths.
/// All flags default to false (disabled) — enable one at a time during validation.
/// Pattern: same approach used for GuiaRemision in Phase 2.
/// </summary>
public static class FeatureFlags
{
    /// <summary>
    /// When true, the "Comprobantes" menu opens <see cref="EVANS.UI.WinForms.Comprobante.frmComprobante"/>
    /// (new C# WinForms form backed by Application handlers + Dapper).
    /// When false, falls through to the legacy VB path.
    /// </summary>
    public static bool ComprobanteV2Enabled { get; set; } = false;
}
