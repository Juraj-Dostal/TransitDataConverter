using TDCLibrary.JdfModel.Enums;

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
    /// Pevné kódy (NEPOVINNÉ)
    /// Pole až 2 pevných kódov
    /// </summary>
    public PevnyKodOznacenie?[] PevneKody { get; set; } = new PevnyKodOznacenie?[2];
    
    /// <summary>
    /// Kilometry (VOLITEĽNÉ)
    /// </summary>
    public double? Kilometry { get; set; }
    
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
    
    public static string ConvertTime(string time)
    {
        var parts = time.Split(':');
        if (parts.Length == 3)
        {
            int hours = int.Parse(parts[0]);
            int minutes = int.Parse(parts[1]);
            int seconds = int.Parse(parts[2]);
            return $"{hours:D2}{minutes:D2}";
        }
        return "0000";
    }
}