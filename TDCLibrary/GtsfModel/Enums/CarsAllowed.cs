namespace TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Možnosť pripojenia osobných áut (trips.txt - cars_allowed)
/// Označuje, či sa vozidlá s osobnými autami môžu naložiť na túto jazdu
/// </summary>
public enum CarsAllowed
{
    /// <summary>
    /// Žiadna informácia - No information
    /// Nie je známe, či sú osobné autá povolené
    /// </summary>
    NoInformation = 0,
    
    /// <summary>
    /// Autá sú povolené - Cars allowed
    /// Vozidlá s osobnými autami môžu byť naložené na túto jazdu
    /// </summary>
    Allowed = 1,
    
    /// <summary>
    /// Autá nie sú povolené - Cars not allowed
    /// Vozidlá s osobnými autami nemôžu byť naložené na túto jazdu
    /// </summary>
    NotAllowed = 2
}
