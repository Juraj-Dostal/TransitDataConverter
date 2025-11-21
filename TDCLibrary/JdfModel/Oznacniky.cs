namespace TDCLibrary.JdfModel;

/// <summary>
/// Označniky zastávok (Oznacniky.txt - NEPOVINNÝ súbor)
/// Soubor Oznacniky slouží jako číselník označníků pro předávanou dávku. Označník je vázaný na právě jednu zastávku (vazba do Zastavky).
/// Označníky jedné zastávky jsou rozlišeny polem Kód označníku, hodnota musí odpovídat celostátnímu registru zastávek CIS JŘ
/// </summary>
public class Oznacniky
{
    /// <summary>
    /// Číslo zastávky (POVINNÉ)
    /// </summary>
    public int CisloZastavky { get; set; }
    
    /// <summary>
    /// Kód označníku (POVINNÉ)
    /// </summary>
    public int KodOznacniku { get; set; }
    
    /// <summary>
    /// Název (VOLITEĽNÉ)
    /// </summary>
    public string? Nazov { get; set; }
    
    /// <summary>
    /// Směr/popis (VOLITEĽNÉ)
    /// </summary>
    public string? SmerPopis { get; set; }
    
    /// <summary>
    /// Stanoviště (VOLITEĽNÉ)
    /// </summary>
    public string? Stanoviste { get; set; }
    
    /// <summary>
    /// Rezerva (VOLITEĽNÉ)
    /// </summary>
    public string? Rezerva { get; set; }
}