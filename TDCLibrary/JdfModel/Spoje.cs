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
    
    // Pomocné vlastnosti pre GUI binding - zobrazujú názov enumu a číslo
    public string? PevnyKod1 => PevneKody[0].HasValue ? $"{PevneKody[0].Value} ({PevnyKodExtensions.DajCislo(PevneKody[0].Value)})" : null;
    public string? PevnyKod2 => PevneKody[1].HasValue ? $"{PevneKody[1].Value} ({PevnyKodExtensions.DajCislo(PevneKody[1].Value)})" : null;
    public string? PevnyKod3 => PevneKody[2].HasValue ? $"{PevneKody[2].Value} ({PevnyKodExtensions.DajCislo(PevneKody[2].Value)})" : null;
    public string? PevnyKod4 => PevneKody[3].HasValue ? $"{PevneKody[3].Value} ({PevnyKodExtensions.DajCislo(PevneKody[3].Value)})" : null;
    public string? PevnyKod5 => PevneKody[4].HasValue ? $"{PevneKody[4].Value} ({PevnyKodExtensions.DajCislo(PevneKody[4].Value)})" : null;
    public string? PevnyKod6 => PevneKody[5].HasValue ? $"{PevneKody[5].Value} ({PevnyKodExtensions.DajCislo(PevneKody[5].Value)})" : null;
    public string? PevnyKod7 => PevneKody[6].HasValue ? $"{PevneKody[6].Value} ({PevnyKodExtensions.DajCislo(PevneKody[6].Value)})" : null;
    public string? PevnyKod8 => PevneKody[7].HasValue ? $"{PevneKody[7].Value} ({PevnyKodExtensions.DajCislo(PevneKody[7].Value)})" : null;
    public string? PevnyKod9 => PevneKody[8].HasValue ? $"{PevneKody[8].Value} ({PevnyKodExtensions.DajCislo(PevneKody[8].Value)})" : null;
    public string? PevnyKod10 => PevneKody[9].HasValue ? $"{PevneKody[9].Value} ({PevnyKodExtensions.DajCislo(PevneKody[9].Value)})" : null;
}
