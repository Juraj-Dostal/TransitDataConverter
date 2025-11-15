namespace TDCLibrary.GtsfModel;

using TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Predstavuje frekvencie jázd (frequencies.txt - VOLITEĽNÝ súbor).
/// Používa sa pre služby, ktoré fungujú s pravidelnou frekvenciou namiesto pevného rozvrhu.
/// Napr. metro každých 10 minút.
/// </summary>
public class Frequency
{
    /// <summary>
    /// Identifikátor jazdy (POVINNÉ)
    /// </summary>
    public string TripId { get; set; } = string.Empty;
    
    /// <summary>
    /// Čas začiatku intervalu (POVINNÉ)
    /// Formát: HH:MM:SS
    /// </summary>
    public string StartTime { get; set; } = string.Empty;
    
    /// <summary>
    /// Čas konca intervalu (POVINNÉ)
    /// Formát: HH:MM:SS
    /// </summary>
    public string EndTime { get; set; } = string.Empty;
    
    /// <summary>
    /// Interval medzi odchodmi v sekundách (POVINNÉ)
    /// Napr. 600 pre 10 minút
    /// </summary>
    public int HeadwaySecs { get; set; }
    
    /// <summary>
    /// Typ rozvrhu (VOLITEĽNÉ)
    /// </summary>
    public FrequencyExactTimes? ExactTimes { get; set; }
}
