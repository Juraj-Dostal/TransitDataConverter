namespace TDCLibrary.GtsfModel;

using TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Predstavuje zastávku, stanicu alebo vstup/výstup stanice (stops.txt - POVINNÝ súbor).
/// Zastávky sú miesta, kde vozidlá zastavujú pre nástup alebo výstup cestujúcich.
/// </summary>
public class Stop
{
    /// <summary>
    /// Jedinečný identifikátor zastávky (POVINNÉ)
    /// Viaceré trasy môžu mať rovnaký identifikátor
    /// </summary>
    public string StopId { get; set; } = string.Empty;
    
    /// <summary>
    /// Kód zastávky pre cestujúcich (VOLITEĽNÉ)
    /// Krátky text alebo číslo, ktoré identifikuje zastávku
    /// </summary>
    public string? StopCode { get; set; }
    
    /// <summary>
    /// Názov zastávky (PODMIENEČNE POVINNÉ)
    /// </summary>
    public string? StopName { get; set; }
    
    /// <summary>
    /// Text-to-Speech názov zastávky (VOLITEĽNÉ)
    /// Čitateľná verzia názvu pre TTS systémy
    /// </summary>
    public string? TtsStopName { get; set; }
    
    /// <summary>
    /// Popis zastávky - užitočné info pre cestujúcich (VOLITEĽNÉ)
    /// </summary>
    public string? StopDesc { get; set; }
    
    /// <summary>
    /// Zemepisná šírka zastávky (PODMIENEČNE POVINNÉ)
    /// WGS84 formát
    /// </summary>
    public double? StopLat { get; set; }
    
    /// <summary>
    /// Zemepisná dĺžka zastávky (PODMIENEČNE POVINNÉ)
    /// WGS84 formát
    /// </summary>
    public double? StopLon { get; set; }
    
    /// <summary>
    /// Identifikátor zóny (VOLITEĽNÉ)
    /// Používa sa pre výpočet cestovného
    /// </summary>
    public string? ZoneId { get; set; }
    
    /// <summary>
    /// URL webovej stránky o zastávke (VOLITEĽNÉ)
    /// </summary>
    public string? StopUrl { get; set; }
    
    /// <summary>
    /// Typ miesta (VOLITEĽNÉ)
    /// </summary>
    public LocationType? LocationType { get; set; }
    
    /// <summary>
    /// ID rodičovskej stanice (PODMIENEČNE POVINNÉ)
    /// Používa sa keď je location_type 2, 3 alebo 4
    /// </summary>
    public string? ParentStation { get; set; }
    
    /// <summary>
    /// Časová zóna zastávky (VOLITEĽNÉ)
    /// </summary>
    public string? StopTimezone { get; set; }
    
    /// <summary>
    /// Informácia o prístupnosti pre vozíčkarov (VOLITEĽNÉ)
    /// </summary>
    public WheelchairAccessibility? WheelchairBoarding { get; set; }
    
    /// <summary>
    /// Úroveň pre viacúrovňové stanice (VOLITEĽNÉ)
    /// </summary>
    public string? LevelId { get; set; }
    
    /// <summary>
    /// Kód platformy (VOLITEĽNÉ)
    /// </summary>
    public string? PlatformCode { get; set; }
}
