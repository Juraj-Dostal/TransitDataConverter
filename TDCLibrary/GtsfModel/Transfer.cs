namespace TDCLibrary.GtsfModel;

using TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Predstavuje prestup medzi trasami (transfers.txt - VOLITEĽNÝ súbor).
/// Definuje pravidlá pre prestupy medzi zastávkami v rôznych trasách.
/// </summary>
public class Transfer
{
    /// <summary>
    /// Identifikátor výstupnej zastávky (POVINNÉ)
    /// </summary>
    public string FromStopId { get; set; } = string.Empty;
    
    /// <summary>
    /// Identifikátor nástupnej zastávky (POVINNÉ)
    /// </summary>
    public string ToStopId { get; set; } = string.Empty;
    
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
