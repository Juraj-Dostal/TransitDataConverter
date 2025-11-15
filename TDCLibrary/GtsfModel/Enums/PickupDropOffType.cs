namespace TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Typ nástupu alebo výstupu cestujúcich
/// </summary>
public enum PickupDropOffType
{
    /// <summary>
    /// Pravidelný nástup/výstup - Regular
    /// Pravidelne naplánovaný nástup alebo výstup
    /// </summary>
    Regular = 0,
    
    /// <summary>
    /// Žiadny nástup/výstup - Not available
    /// Nástup alebo výstup nie je na tejto zastávke možný
    /// </summary>
    NotAvailable = 1,
    
    /// <summary>
    /// Je potrebné telefonovať agentúre - Must phone agency
    /// Cestujúci musí zavolať agentúre, aby dohodol nástup alebo výstup
    /// </summary>
    MustPhoneAgency = 2,
    
    /// <summary>
    /// Je potrebné koordinovať s vodičom - Must coordinate with driver
    /// Cestujúci musí sa dohovoriť s vodičom o nástupe alebo výstupe
    /// </summary>
    MustCoordinateWithDriver = 3
}
