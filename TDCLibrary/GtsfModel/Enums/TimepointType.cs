namespace TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Presnosť časov
/// </summary>
public enum TimepointType
{
    /// <summary>
    /// Aproximatívne časy - Approximate
    /// Časy sú približné a nie sú presne určené
    /// </summary>
    Approximate = 0,
    
    /// <summary>
    /// Presné časy - Exact
    /// Vozidlo dodržiava presné stanovené časy
    /// </summary>
    Exact = 1
}
