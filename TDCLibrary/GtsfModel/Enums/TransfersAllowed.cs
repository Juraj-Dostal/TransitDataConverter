namespace TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Počet povolených prestupov
/// </summary>
public enum TransfersAllowed
{
    /// <summary>
    /// Žiadne prestupy - No transfers
    /// Žiadne prestupy nie sú povolené
    /// </summary>
    NoTransfers = 0,
    
    /// <summary>
    /// Jeden prestup - One transfer
    /// Cestujúci sa môže prestúpiť raz
    /// </summary>
    OneTransfer = 1,
    
    /// <summary>
    /// Dva prestupy - Two transfers
    /// Cestujúci sa môže prestúpiť dvakrát
    /// </summary>
    TwoTransfers = 2,
    
    /// <summary>
    /// Neomedzené prestupy - Unlimited transfers
    /// Neobmedzený počet prestupov je povolený
    /// </summary>
    Unlimited = -1
}
