using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary.JdfModel;

/// <summary>
/// Soubor Zaslinky obsahuje seznam zastávek všech linek předávaných v dávce (Zaslinky.txt - POVINNÝ súbor)
/// Jednoznačnost je dána číslem linky, rozlišením linky a tarifním číslem zastávky, které vyjadřuje posloupnost zastávek v rámci linky.
/// </summary>
public class Zaslinky
{
    /// <summary>
    /// Číslo linky (POVINNÉ)
    /// </summary>
    public int CisloLinky { get; set; }
    
    /// <summary>
    /// Číslo tarifní (POVINNÉ)
    /// arifním číslem zastávky, které vyjadřuje posloupnost zastávek v rámci linky
    /// </summary>
    public int CisloTarifni { get; set; }
    
    /// <summary>
    /// Tarifní pásmo (NEPOVINNÉ)
    /// </summary>
    public string? TarifniPasmo { get; set; }
    
    /// <summary>
    /// Číslo zastávky (POVINNÉ)
    /// </summary>
    public int CisloZastavky { get; set; }
    
    /// <summary>
    /// Průměrná doba (NEPOVINNÉ)
    /// </summary>
    public string? PriemernaDoba { get; set; }
    
    /// <summary>
    /// Pevné kódy (NEPOVINNÉ)
    /// Pole až 3 pevných kódov
    /// </summary>
    public PevnyKodOznacenie?[] PevneKody { get; set; } = new PevnyKodOznacenie?[3];
    
    /// <summary>
    /// Rozlišení linky (POVINNÉ)
    /// </summary>
    public int RozlisenieLinky { get; set; }
}