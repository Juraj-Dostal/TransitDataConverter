namespace TDCLibrary.JdfModel;

/// <summary>
/// Verzia jednotného dátového formátu (VerzeJDF.txt - POVINNÝ súbor)
/// </summary>
public class VerzeJDF
{
    /// <summary>
    /// Číslo verze JDF (POVINNÉ)
    /// </summary>
    public string VerziaJDF { get; set; } = "1.10";
    
    /// <summary>
    /// Číslo DÚ (VOLITEĽNÉ)
    /// </summary>
    public int? CisloDU { get; set; }
    
    /// <summary>
    /// Okres/Kraj (VOLITEĽNÉ)
    /// </summary>
    public string? OkresKraj { get; set; }

    /// <summary>
    /// Identifikace dávky (VOLITEĽNÉ)
    /// </summary>
    public string? IdentikaciaDat { get; set; }
    
    /// <summary>
    /// Datum výroby dávky (POVINNÉ)
    /// Tvar DDMMRRRR
    /// </summary>
    public string DatumVyrobyDat { get; set; }
    
    /// <summary>
    /// Jméno (VOLITEĽNÉ)
    /// </summary>
    public string? Meno { get; set; }
}