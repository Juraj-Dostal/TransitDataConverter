using TDCLibrary.GtfsModel.Enums;

namespace TDCLibrary.GtfsModel;

/// <summary>
/// Predstavuje časy príchodov a odchodov pre zastávky na každej jazde (stop_times.txt - POVINNÝ súbor).
/// Definuje kedy vozidlo príde a odíde z každej zastávky počas jazdy.
/// </summary>
public class StopTime
{
    /// <summary>
    /// Identifikátor jazdy (POVINNÉ)
    /// </summary>
    public string TripId { get; set; } = string.Empty;
    
    /// <summary>
    /// Čas príchodu (PODMIENEČNE POVINNÉ)
    /// Formát: HH:MM:SS (môže byť > 24:00:00 pre časy po polnoci)
    /// </summary>
    public string? ArrivalTime { get; set; }
    
    /// <summary>
    /// Čas odchodu (PODMIENEČNE POVINNÉ)
    /// Formát: HH:MM:SS
    /// </summary>
    public string? DepartureTime { get; set; }
    
    /// <summary>
    /// Identifikátor zastávky (POVINNÉ)
    /// </summary>
    public string StopId { get; set; } = string.Empty;
    
    /// <summary>
    /// Poradie zastávky v rámci jazdy (POVINNÉ)
    /// Začína od 0 a zvyšuje sa
    /// </summary>
    public int StopSequence { get; set; }
    
    /// <summary>
    /// Text zobrazený cestujúcim pre túto zastávku (VOLITEĽNÉ)
    /// </summary>
    public string? StopHeadsign { get; set; }
    
    /// <summary>
    /// Typ nástupu (VOLITEĽNÉ)
    /// </summary>
    public PickupDropOffType? PickupType { get; set; }
    
    /// <summary>
    /// Typ výstupu (VOLITEĽNÉ)
    /// </summary>
    public PickupDropOffType? DropOffType { get; set; }
    
    /// <summary>
    /// Priebežný nástup (VOLITEĽNÉ)
    /// </summary>
    public ContinuousPickupDropOff? ContinuousPickup { get; set; }
    
    /// <summary>
    /// Priebežný výstup (VOLITEĽNÉ)
    /// </summary>
    public ContinuousPickupDropOff? ContinuousDropOff { get; set; }
    
    /// <summary>
    /// Skutočná prejdená vzdialenosť od prvej zastávky (VOLITEĽNÉ)
    /// V jednotkách použitých v shapes.txt
    /// </summary>
    public double? ShapeDistTraveled { get; set; }
    
    /// <summary>
    /// Indikuje presnosť času (VOLITEĽNÉ)
    /// </summary>
    public TimepointType? Timepoint { get; set; }
}
