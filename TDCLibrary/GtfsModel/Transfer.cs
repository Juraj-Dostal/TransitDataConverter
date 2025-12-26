using TDCLibrary.GtfsModel.Enums;

namespace TDCLibrary.GtfsModel;

/// <summary>
/// Predstavuje prestup medzi trasami (transfers.txt - VOLITEĽNÝ súbor).
/// Definuje pravidlá pre prestupy medzi zastávkami v rôznych trasách.
/// </summary>
public class Transfer
{
    /// <summary>
    /// Identifikátor výstupnej zastávky (PODMIENEČNE POVINNÉ)
    /// Povinné ak from_trip_id nie je zadané
    /// </summary>
    public string FromStopId { get; set; } = string.Empty;
    
    /// <summary>
    /// Identifikátor nástupnej zastávky (PODMIENEČNE POVINNÉ)
    /// Povinné ak to_trip_id nie je zadané
    /// </summary>
    public string ToStopId { get; set; } = string.Empty;
    
    /// <summary>
    /// Identifikátor výstupnej trasy (VOLITEĽNÉ)
    /// Ak je uvedené, prestup sa vzťahuje len na príchody z tejto trasy
    /// </summary>
    public string? FromRouteId { get; set; }
    
    /// <summary>
    /// Identifikátor nástupnej trasy (VOLITEĽNÉ)
    /// Ak je uvedené, prestup sa vzťahuje len na odchody na túto trasu
    /// </summary>
    public string? ToRouteId { get; set; }
    
    /// <summary>
    /// Identifikátor výstupnej jazdy (PODMIENEČNE POVINNÉ)
    /// Povinné ak from_stop_id nie je zadané
    /// </summary>
    public string? FromTripId { get; set; }
    
    /// <summary>
    /// Identifikátor nástupnej jazdy (PODMIENEČNE POVINNÉ)
    /// Povinné ak to_stop_id nie je zadané
    /// </summary>
    public string? ToTripId { get; set; }
    
    /// <summary>
    /// Typ prestupu (POVINNÉ)
    /// </summary>
    public TransferType TransferType { get; set; }
    
    /// <summary>
    /// Minimálny čas potrebný na prestup v sekundách (VOLITEĽNÉ)
    /// Použije sa keď transfer_type = MinimumTimeRequired
    /// </summary>
    public int? MinTransferTime { get; set; }
}
