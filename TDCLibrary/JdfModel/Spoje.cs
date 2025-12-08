using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary.JdfModel;

/// <summary>
/// Soubor spoje obsahuje pro každý spoj jeden záznam (Spoje.txt - POVINNÝ súbor)
/// </summary>
public class Spoje
{
    /// <summary>
    /// Číslo linky (POVINNÉ)
    /// </summary>
    public int CisloLinky { get; set; }
    
    /// <summary>
    /// Číslo spoje (POVINNÉ)
    /// liché číslo u spojů vedených ve směru vedení linky
    /// sudé číslo u spojů vedených ve směru zpět
    /// </summary>
    public int Cislo { get; set; }
    
    /// <summary>
    /// Pevné kódy (NEPOVINNÉ)
    /// Pole až 10 pevných kódov
    /// </summary>
    public PevnyKodOznacenie?[] PevneKody { get; set; } = new PevnyKodOznacenie?[10];
    
    /// <summary>
    /// Kód skupiny spojů (NEPOVINNÉ)
    /// </summary>
    public int? KodSkupinySpoju { get; set; }
    
    /// <summary>
    /// Rozlišení linky (POVINNÉ)
    /// </summary>
    public int RozliseniLinky { get; set; }
}