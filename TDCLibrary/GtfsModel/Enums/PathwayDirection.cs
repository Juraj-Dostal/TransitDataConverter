namespace TDCLibrary.GtfsModel.Enums;

/// <summary>
/// Smer chodníka
/// </summary>
public enum PathwayDirection
{
    /// <summary>
    /// Jednosmerný - Unidirectional
    /// Chodník je jednosmerný (len z from_stop_id do to_stop_id)
    /// </summary>
    Unidirectional = 0,
    
    /// <summary>
    /// Obojsmerný - Bidirectional
    /// Chodník môže byť použitý v oboch smeroch
    /// </summary>
    Bidirectional = 1
}
