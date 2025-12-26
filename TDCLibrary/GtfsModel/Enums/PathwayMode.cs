namespace TDCLibrary.GtfsModel.Enums;

/// <summary>
/// Typ chodníka v stanici
/// </summary>
public enum PathwayMode
{
    /// <summary>
    /// Chodník - Walkway
    /// Chodný chodník bez schodov alebo eskalátora
    /// </summary>
    Walkway = 1,
    
    /// <summary>
    /// Schody - Stairs
    /// Schody
    /// </summary>
    Stairs = 2,
    
    /// <summary>
    /// Pohyblivé schody (eskalátor) - Moving sidewalk/travelator
    /// Eskalátor alebo pohyblivý chodník
    /// </summary>
    MovingSidewalk = 3,
    
    /// <summary>
    /// Eskalátor - Escalator
    /// Eskalátor
    /// </summary>
    Escalator = 4,
    
    /// <summary>
    /// Výťah - Elevator
    /// Výťah
    /// </summary>
    Elevator = 5,
    
    /// <summary>
    /// Platobná brána - Fare gate
    /// Brána kde je potrebné použiť lístok na prechod
    /// </summary>
    FareGate = 6,
    
    /// <summary>
    /// Východová brána - Exit gate
    /// Brána, ktorá oddeluje plateného od neplateného územia stanice
    /// </summary>
    ExitGate = 7
}
