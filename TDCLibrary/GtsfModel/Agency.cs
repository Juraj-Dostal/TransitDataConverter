namespace TDCLibrary.GtsfModel;

using TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Predstavuje dopravnú agentúru (agency.txt - POVINNÝ súbor).
/// Obsahuje informácie o dopravných spoločnostiach, ktoré poskytujú služby uvedené v tomto datasete.
/// </summary>
public class Agency
{
    /// <summary>
    /// Jedinečný identifikátor agentúry (VOLITEĽNÉ ak je len jedna agentúra, inak POVINNÉ)
    /// </summary>
    public string? AgencyId { get; set; }
    
    /// <summary>
    /// Úplný názov dopravnej agentúry (POVINNÉ)
    /// </summary>
    public string AgencyName { get; set; } = string.Empty;
    
    /// <summary>
    /// URL webovej stránky agentúry (POVINNÉ)
    /// </summary>
    public string AgencyUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Časová zóna agentúry (POVINNÉ)
    /// Formát: TZ database timezone names (napr. "Europe/Bratislava")
    /// </summary>
    public string AgencyTimezone { get; set; } = string.Empty;
    
    /// <summary>
    /// Primárny jazyk používaný agentúrou (VOLITEĽNÉ)
    /// Formát: ISO 639-1 kód (napr. "sk", "en")
    /// </summary>
    public string? AgencyLang { get; set; }
    
    /// <summary>
    /// Telefónne číslo pre hlasovú podporu (VOLITEĽNÉ)
    /// </summary>
    public string? AgencyPhone { get; set; }
    
    /// <summary>
    /// URL stránky pre nákup lístkov (VOLITEĽNÉ)
    /// </summary>
    public string? AgencyFareUrl { get; set; }
    
    /// <summary>
    /// Email kontakt na zákaznícku podporu (VOLITEĽNÉ)
    /// </summary>
    public string? AgencyEmail { get; set; }
    
    /// <summary>
    /// Podpora bezkontaktných platobných kariet cEMV (VOLITEĽNÉ)
    /// Označuje, či cestujúci môžu použiť cEMV karty (contactless EMV) ako platobné médium pre jazdy
    /// 0 alebo prázdne = žiadna informácia, 1 = podporované, 2 = nepodporované
    /// </summary>
    public CemvSupport? CemvSupport { get; set; }
}
