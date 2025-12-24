using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary.JdfModel;

/// <summary>
/// Časové kódy spojov - (Caskody.txt - POVINNÝ súbor)
/// Soubor Caskody je určen k předávání údajů o časovém rozsahu provozu jednotlivých spojů v konkrétních datově
/// určených dnech nebo v určitých intervalech dnů a o značkách informativních
/// </summary>
public class Caskody
{
    /// <summary>
    /// Číslo linky (POVINNÉ)
    /// </summary>
    public int CisloLinky { get; set; }
    
    /// <summary>
    /// Číslo spoje (POVINNÉ)
    /// </summary>
    public int CisloSpoje { get; set; }
    
    /// <summary>
    /// Číslo časového kódu (POVINNÉ)
    /// </summary>
    public int Cislo { get; set; }
    
    /// <summary>
    /// Označení časového kódu (POVINNÉ)
    /// </summary>
    public int Oznacenie { get; set; }
    
    /// <summary>
    /// Typ časového kódu (NEPOVINNÉ)
    /// </summary>
    public TypCasKod? Typ { get; set; }
    
    /// <summary>
    /// Datum od (NEPOVINNÉ)
    /// Tvar datum: DDMMRRRR
    /// </summary>
    public string? DatumOd { get; set; }
    
    /// <summary>
    /// Datum do (NEPOVINNÉ)
    /// Tvar datum: DDMMRRRR
    /// </summary>
    public string? DatumDo { get; set; }
    
    /// <summary>
    /// Poznámka (NEPOVINNÉ)
    /// </summary>
    public string? Poznamka { get; set; }
    
    /// <summary>
    /// Rozlišení linky (POVINNÉ)
    /// </summary>
    public int RozlisenieLinky { get; set; }
    
}