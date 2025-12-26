namespace TDCLibrary.GtfsModel.Enums;

/// <summary>
/// Spôsob platby cestovného
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// Platba na palube - Pay on board
    /// Cestovné sa platí na palube vozidla
    /// </summary>
    OnBoard = 0,
    
    /// <summary>
    /// Platba pred nástupom - Pay before boarding
    /// Cestovné sa musí zaplatiť pred nástupom do vozidla
    /// </summary>
    BeforeBoarding = 1
}
