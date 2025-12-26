namespace TDCLibrary.GtfsModel.Enums;

/// <summary>
/// Typ priebežného nástupu alebo výstupu
/// </summary>
public enum ContinuousPickupDropOff
{
    /// <summary>
    /// Priebežný nástup/výstup - Continuous
    /// Priebežný nástup alebo výstup je povolený
    /// </summary>
    Continuous = 0,
    
    /// <summary>
    /// Žiadny priebežný nástup/výstup - Not available
    /// Priebežný nástup alebo výstup nie je povolený
    /// </summary>
    NotAvailable = 1,
    
    /// <summary>
    /// Je potrebné telefonovať agentúre - Must phone agency
    /// Cestujúci musí zavolať agentúre pre priebežný nástup/výstup
    /// </summary>
    MustPhoneAgency = 2,
    
    /// <summary>
    /// Je potrebné koordinovať s vodičom - Must coordinate with driver
    /// Cestujúci musí koordinovať s vodičom pre priebežný nástup/výstup
    /// </summary>
    MustCoordinateWithDriver = 3
}
