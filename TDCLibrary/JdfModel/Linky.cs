using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary.JdfModel;

/// <summary>
/// Linky (Linky.txt - POVINNÝ súbor)
/// Soubor Linky obsahuje pro každou verzi linky jeden záznam.
/// </summary>
public class Linky
{
    /// <summary>
    /// Cislo linky (POVINNÉ)
    /// </summary>
    public int Cislo { get; set; }
    
    /// <summary>
    /// Názov linky (POVINNÉ)
    /// </summary>
    public string Nazov { get; set; }
    
    /// <summary>
    /// IČ dopravce (POVINNÉ)
    /// </summary>
    public string IcDopravce { get; set; }
    
    /// <summary>
    /// Typ linky (POVINNÉ)
    /// </summary>
    public TypLinky Typ { get; set; }
    
    /// <summary>
    /// Dopravní prostředek (POVINNÉ)
    /// </summary>
    public DopravnyProstriedok DopravnyProstriedok { get; set; }
    
    /// <summary>
    /// Objížďkový JŘ (POVINNÉ)
    /// </summary>
    public bool ObjizdkovyJR { get; set; }
    
    /// <summary>
    /// Seskupení spojů (POVINNÉ)
    /// </summary>
    public bool SeskupenieSpojov { get; set; }
    
    /// <summary>
    /// Použití označníků (POVINNÉ)
    /// </summary>
    public bool PouzitieOznacnikov { get; set; }
    
    /// <summary>
    /// Rezerva (NEPOVINNÉ)
    /// </summary>
    public string? Rezerva { get; set; }
    
    /// <summary>
    /// Číslo licence (NEPOVINNÉ)
    /// </summary>
    public string? CisloLicencie { get; set; }
    
    /// <summary>
    /// Platnost licencie od (NEPOVINNÉ)
    /// </summary>
    public string? PlatnostLicencieOd { get; set; }
    
    /// <summary>
    /// Platnost licencie do (NEPOVINNÉ)
    /// </summary>
    public string? PlatnostLicencieDo { get; set; }
    
    /// <summary>
    /// Platnost JŘ od (POVINNÉ)
    /// </summary>
    public string PlatnostJROd { get; set; }
    
    /// <summary>
    /// Platnost JŘ do (NEPOVINNÉ)
    /// </summary>
    public string PlatnostJRDo { get; set; }
    
    /// <summary>
    /// Rozlišení dopravce (POVINNÉ)
    /// </summary>
    public int RozlisenieDopravcu { get; set; }
    
    /// <summary>
    /// Rozlišení linky (POVINNÉ)
    /// </summary>
    public int RozlisenieLinky { get; set; }
    
}