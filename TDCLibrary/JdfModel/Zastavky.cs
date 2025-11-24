namespace TDCLibrary.JdfModel;

/// <summary>
/// Zastávky (Zastavky.txt - POVINNÝ súbor)
/// Soubor Zastavky slouží jako číselník zastávek pro předávanou dávku. Vazba je realizována přes číslo zastávky (ze souborů Zasspoje a Zaslinky).
/// uveden název hraničního přechodu, který slouží pouze pro účely pasového a celního odbavení, uvede se do atributů v souboru Zaslinky pevný kód "$" (CLO).
/// Tento pevný kód se neuvádí, pokud je na hraničním přechodu zastávka pro nástup a výstup cestujících.
/// Pole Stát je povinné vždy. Pole Blízká obec je povinné jen pro zastávky na území ČR a SR.
/// </summary>
public class Zastavky
{
    /// <summary>
    /// Cislo zastávky (POVINNÉ)
    /// </summary>
    public int Cislo { get; set; }
    
    /// <summary>
    /// Název obce (POVINNÉ)
    /// </summary>
    public string NazovObce { get; set; }
    
    /// <summary>
    /// Část obce (VOLITEĽNÉ)
    /// </summary>
    public string? CastObce { get; set; }
    
    /// <summary>
    /// Bližší místo (VOLITEĽNÉ)
    /// </summary>
    public string? BlizkeMiesto { get; set; }
    
    /// <summary>
    /// Blízká obec (POVINNÉ, ak je štát CZ alebo SK)
    /// </summary>
    public string? BlizkaObec { get; set; }
    
    /// <summary>
    /// Stát (POVINNÉ)
    /// </summary>
    public string Stat { get; set; }
    
    /// <summary>
    /// Pevny kód, väzba do PevnyKod (VOLITEĽNÉ)
    /// </summary>
    public string? PevnyKod1 { get; set; }
    public string? PevnyKod2 { get; set; }
    public string? PevnyKod3 { get; set; }
    public string? PevnyKod4 { get; set; }
    public string? PevnyKod5 { get; set; }
    public string? PevnyKod6 { get; set; }
}