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
    /// Pevny kód (NEPOVINNÉ)
    /// </summary>
    public int? PevnyKod1 { get; set; }
    public int? PevnyKod2 { get; set; }
    public int? PevnyKod3 { get; set; }
    public int? PevnyKod4 { get; set; }
    public int? PevnyKod5 { get; set; }
    public int? PevnyKod6 { get; set; }
    public int? PevnyKod7 { get; set; }
    public int? PevnyKod8 { get; set; }
    public int? PevnyKod9 { get; set; }
    public int? PevnyKod10 { get; set; }
    
    /// <summary>
    /// Kód skupiny spojů (NEPOVINNÉ)
    /// </summary>
    public int? KodSkupinySpoju { get; set; }
    
    /// <summary>
    /// Rozlišení linky (POVINNÉ)
    /// </summary>
    public int RozliseniLinky { get; set; }
}