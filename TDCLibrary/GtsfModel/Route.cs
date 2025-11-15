namespace TDCLibrary.GtsfModel;

using TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Predstavuje trasu (routes.txt - POVINNÝ súbor).
/// Trasa je skupina jázd, ktoré sa zobrazujú cestujúcim ako jedna služba.
/// </summary>
public class Route
{
    /// <summary>
    /// Jedinečný identifikátor trasy (POVINNÉ)
    /// </summary>
    public string RouteId { get; set; } = string.Empty;
    
    /// <summary>
    /// ID agentúry pre túto trasu (PODMIENEČNE POVINNÉ)
    /// Povinné ak dataset obsahuje viac agentúr
    /// </summary>
    public string? AgencyId { get; set; }
    
    /// <summary>
    /// Krátky názov trasy (PODMIENEČNE POVINNÉ)
    /// Napr. "32", "100", "Green Line"
    /// </summary>
    public string? RouteShortName { get; set; }
    
    /// <summary>
    /// Úplný názov trasy (PODMIENEČNE POVINNÉ)
    /// </summary>
    public string? RouteLongName { get; set; }
    
    /// <summary>
    /// Popis trasy (VOLITEĽNÉ)
    /// </summary>
    public string? RouteDesc { get; set; }
    
    /// <summary>
    /// Typ trasy (POVINNÉ)
    /// </summary>
    public RouteType RouteType { get; set; }
    
    /// <summary>
    /// URL webovej stránky trasy (VOLITEĽNÉ)
    /// </summary>
    public string? RouteUrl { get; set; }
    
    /// <summary>
    /// Farba trasy v hexadecimálnom formáte (VOLITEĽNÉ)
    /// Napr. "FF0000" pre červenú
    /// </summary>
    public string? RouteColor { get; set; }
    
    /// <summary>
    /// Farba textu v hexadecimálnom formáte (VOLITEĽNÉ)
    /// Napr. "FFFFFF" pre bielu
    /// </summary>
    public string? RouteTextColor { get; set; }
    
    /// <summary>
    /// Poradie trasy pri zoraďovaní (VOLITEĽNÉ)
    /// Nižšie čísla = vyššia priorita
    /// </summary>
    public int? RouteSortOrder { get; set; }
    
    /// <summary>
    /// Indikuje či je nástup možný v ktoromkoľvek bode trasy (VOLITEĽNÉ)
    /// </summary>
    public ContinuousPickupDropOff? ContinuousPickup { get; set; }
    
    /// <summary>
    /// Indikuje či je výstup možný v ktoromkoľvek bode trasy (VOLITEĽNÉ)
    /// </summary>
    public ContinuousPickupDropOff? ContinuousDropOff { get; set; }
    
    /// <summary>
    /// ID siete, do ktorej trasa patrí (VOLITEĽNÉ)
    /// Používa sa pre zoskupenie trás do siete
    /// </summary>
    public string? NetworkId { get; set; }
    
    /// <summary>
    /// Podpora cEMV pre jazdy na tejto trase (VOLITEĽNÉ, rozšírenie)
    /// 0 alebo prázdne = žiadna informácia, 1 = cEMV podporované, 2 = cEMV nepodporované.
    /// Použiť len ak dataset poskytuje tento stĺpec v routes.txt.
    /// </summary>
    public CemvSupport? CemvSupport { get; set; }
}