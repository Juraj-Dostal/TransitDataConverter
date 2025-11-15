namespace TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Prístupnosť pre vozíčkarov
/// </summary>
public enum WheelchairAccessibility
{
    /// <summary>
    /// Žiadna informácia o prístupnosti - No accessibility information
    /// Nie sú dostupné informácie o prístupnosti
    /// </summary>
    NoInformation = 0,
    
    /// <summary>
    /// Prístupné pre vozíčkarov - Accessible
    /// Vozidlo/zastávka je čiastočne alebo úplne prístupné pre vozíčkarov
    /// </summary>
    Accessible = 1,
    
    /// <summary>
    /// Neprístupné pre vozíčkarov - Not accessible
    /// Nie je možný nástup pre cestujúcich na vozíčkoch
    /// </summary>
    NotAccessible = 2
}
