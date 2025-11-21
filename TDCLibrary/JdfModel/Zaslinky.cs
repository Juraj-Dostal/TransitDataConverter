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
    /// Pevný kód (NEPOVINNÉ)
    /// </summary>
    public string? PevnyKod1 { get; set; }
    public string? PevnyKod2 { get; set; }
    public string? PevnyKod3 { get; set; }
    
    /// <summary>
    /// Rozlišení linky (POVINNÉ)
    /// </summary>
    public int RozlisenieLinky { get; set; }
}