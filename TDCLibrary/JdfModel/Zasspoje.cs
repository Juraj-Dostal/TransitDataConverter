namespace TDCLibrary.JdfModel;

/// <summary>
/// Zastávky spojov (Zasspoje.txt - POVINNÝ súbor)
/// Soubor Zasspoje obsahuje pro každý spoj a každou zastávku linky jeden záznam
/// Jednoznačnost záznamu je určena číslem linky, rozlišením linky, číslem spoje a tarifním číslem zastávky
/// </summary>
public class Zasspoje
{
    /// <summary>
    /// Cislo linky (POVINNÉ)
    /// </summary>
    public int CisloLinky { get; set; }
    
    /// <summary>
    /// Cislo spoje (POVINNÉ)
    /// </summary>
    public int CisloSpoje { get; set; }
    
    /// <summary>
    /// Cislo tarifní (POVINNÉ)
    /// </summary>
    public int CisloTarifni { get; set; }
    
    /// <summary>
    /// Cislo zastávky (POVINNÉ)
    /// </summary>
    public int CisloZastavky { get; set; }
    
    /// <summary>
    /// Kód označníku (VOLITEĽNÉ)
    /// </summary>
    public int? KodOznacniku { get; set; }
    
    /// <summary>
    /// Cislo stanoviště (NEPOVINNÉ)
    /// </summary>
    public string? CisloStanoviste { get; set; }
    
    /// <summary>
    /// Pevný kód (NEPOVINNÉ)
    /// </summary>
    public string? PevnyKod1 { get; set; }
    public string? PevnyKod2 { get; set; }
    
    /// <summary>
    /// Kilometry (VOLITEĽNÉ)
    /// </summary>
    public decimal? Kilometry { get; set; }
    
    /// <summary>
    /// Čas příjezdu (POVINNÉ)
    /// číslo, <, |
    /// </summary>
    public string CasPrichodu { get; set; }
    
    /// <summary>
    /// Čas odjezdu (NEPOVINNÉ)
    /// číslo, <, |
    /// </summary>
    public string? CasOdchodu { get; set; }
    
    /// <summary>
    /// Rozlišení linky (POVINNÉ)
    /// </summary>
    public int RozlisenieLinky { get; set; }
}