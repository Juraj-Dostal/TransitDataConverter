using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary.JdfModel;

/// <summary>
/// Dopravné spoločnosti (Dopravci.txt - POVINNÝ súbor)
/// Soubor Dopravci je číselník dopravců linek předávaných v dávce
/// </summary>
public class Dopravci
{
    /// <summary>
    /// IČ (POVINNÉ)
    /// </summary>
    public string IC { get; set; }
    
    /// <summary>
    /// DIČ (VOLITEĽNÉ)
    /// </summary>
    public string? DIC { get; set; }
    
    /// <summary>
    /// Obchodní jméno (POVINNÉ)
    /// </summary>
    public string ObchodnéMeno { get; set; }
    
    /// <summary>
    /// Druh firmy (POVINNÉ)
    /// </summary>
    public DruhFirmy DruhFirmy { get; set; }
    
    /// <summary>
    /// Jméno fyzické osoby (VOLITEĽNÉ)
    /// Pokud je druh firmy Fyzická osoba, je povinné vyplnit
    /// </summary>
    public string? MenoFyzOsoby { get; set; }
    
    /// <summary>
    /// Sídlo (POVINNÉ)
    /// </summary>
    public string Sidlo { get; set; }
    
    /// <summary>
    /// Telefon sídla (POVINNÉ)
    /// </summary>
    public string TelefonSidlo { get; set; }
    
    /// <summary>
    /// Telefon dispečink (VOLITEĽNÉ)
    /// </summary>
    public string? TelefonDispecink { get; set; }
    
    /// <summary>
    /// Telefon informace (VOLITEĽNÉ)
    /// </summary>
    public string? TelefonInformace { get; set; }
    
    /// <summary>
    /// Fax (VOLITEĽNÉ)
    /// </summary>
    public string? Fax { get; set; }
    
    /// <summary>
    /// E-mail (VOLITEĽNÉ)
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Web stranka (VOLITEĽNÉ)
    /// </summary>
    public string? Web { get; set; }
    
    /// <summary>
    /// Rozlišení dopravce (POVINNÉ)
    /// </summary>
    public int RozlisenieDopravcu { get; set; } 
}
