namespace TDCLibrary.GtsfModel;

using TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Predstavuje chodník pre prestup (pathways.txt - VOLITEĽNÝ súbor).
/// Definuje chodníky spájajúce miesta v rámci staníc.
/// Užitočné pre veľké stanice alebo dopravné uzly.
/// </summary>
public class Pathway
{
    /// <summary>
    /// Jedinečný identifikátor chodníka (POVINNÉ)
    /// </summary>
    public string PathwayId { get; set; } = string.Empty;
    
    /// <summary>
    /// Identifikátor počiatočného miesta (POVINNÉ)
    /// </summary>
    public string FromStopId { get; set; } = string.Empty;
    
    /// <summary>
    /// Identifikátor cieľového miesta (POVINNÉ)
    /// </summary>
    public string ToStopId { get; set; } = string.Empty;
    
    /// <summary>
    /// Typ chodníka (POVINNÉ)
    /// </summary>
    public PathwayMode PathwayMode { get; set; }
    
    /// <summary>
    /// Či je chodník obojsmerný (POVINNÉ)
    /// </summary>
    public PathwayDirection IsBidirectional { get; set; }
    
    /// <summary>
    /// Dĺžka chodníka v metroch (VOLITEĽNÉ)
    /// </summary>
    public double? Length { get; set; }
    
    /// <summary>
    /// Čas prechodu v sekundách (VOLITEĽNÉ)
    /// </summary>
    public int? TraversalTime { get; set; }
    
    /// <summary>
    /// Počet schodov (VOLITEĽNÉ)
    /// Kladné pre hore, záporné pre dole
    /// </summary>
    public int? StairCount { get; set; }
    
    /// <summary>
    /// Maximálny sklon chodníka (VOLITEĽNÉ)
    /// </summary>
    public double? MaxSlope { get; set; }
    
    /// <summary>
    /// Minimálna šírka chodníka v metroch (VOLITEĽNÉ)
    /// </summary>
    public double? MinWidth { get; set; }
    
    /// <summary>
    /// Popis návodu pre cestujúcich (VOLITEĽNÉ)
    /// </summary>
    public string? SignpostedAs { get; set; }
    
    /// <summary>
    /// Rovnaký text ako signposted_as, ale keď ide cestujúci opačným smerom (VOLITEĽNÉ)
    /// </summary>
    public string? ReversedSignpostedAs { get; set; }
}
