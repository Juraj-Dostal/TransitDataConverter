namespace TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Možnosť prepravy bicyklov
/// </summary>
public enum BikesAllowed
{
    /// <summary>
    /// Žiadna informácia o preprave bicyklov - No information
    /// Nie sú dostupné informácie o možnosti prepravy bicyklov
    /// </summary>
    NoInformation = 0,
    
    /// <summary>
    /// Bicykle sú povolené - Bikes allowed
    /// Vozidlo môže prepravovať aspoň jeden bicykel
    /// </summary>
    Allowed = 1,
    
    /// <summary>
    /// Bicykle nie sú povolené - Bikes not allowed
    /// Nie je povolené brať bicykle na túto jazdu
    /// </summary>
    NotAllowed = 2
}
