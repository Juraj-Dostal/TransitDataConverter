using TDCLibrary.GtfsModel.Enums;

namespace TDCLibrary.GtfsModel;

/// <summary>
/// Predstavuje jednotlivú jazdu pre trasu (trips.txt - POVINNÝ súbor).
/// Jazda je sekvencia dvoch alebo viacerých zastávok, ktoré sa vykonávajú v konkrétnom čase.
/// </summary>
public class Trip
{
    /// <summary>
    /// ID trasy pre túto jazdu (POVINNÉ)
    /// </summary>
    public string RouteId { get; set; } = string.Empty;
    
    /// <summary>
    /// ID služby, ktoré identifikuje dni, kedy je jazda dostupná (POVINNÉ)
    /// </summary>
    public string ServiceId { get; set; } = string.Empty;
    
    /// <summary>
    /// Jedinečný identifikátor jazdy (POVINNÉ)
    /// </summary>
    public string TripId { get; set; } = string.Empty;
    
    /// <summary>
    /// Text zobrazený cestujúcim identifikujúci jazdu (VOLITEĽNÉ)
    /// Napr. číslo vlaku
    /// </summary>
    public string? TripHeadsign { get; set; }
    
    /// <summary>
    /// Krátky názov jazdy pre cestujúcich (VOLITEĽNÉ)
    /// </summary>
    public string? TripShortName { get; set; }
    
    /// <summary>
    /// Smer jazdy (VOLITEĽNÉ)
    /// 0 = jeden smer, 1 = opačný smer
    /// </summary>
    public DirectionId? DirectionId { get; set; }
    
    /// <summary>
    /// Identifikuje blok, ku ktorému jazda patrí (VOLITEĽNÉ)
    /// Jazdy v jednom bloku môžu byť vykonané s tým istým vozidlom
    /// </summary>
    public string? BlockId { get; set; }
    
    /// <summary>
    /// Identifikuje tvar trasy pre túto jazdu (VOLITEĽNÉ)
    /// Odkazuje na shapes.txt
    /// </summary>
    public string? ShapeId { get; set; }
    
    /// <summary>
    /// Prístupnosť pre vozíčkarov (VOLITEĽNÉ)
    /// </summary>
    public WheelchairAccessibility? WheelchairAccessible { get; set; }
    
    /// <summary>
    /// Možnosť vziať bicykel (VOLITEĽNÉ)
    /// </summary>
    public BikesAllowed? BikesAllowed { get; set; }
    
    /// <summary>
    /// Možnosť pripojenia osobných áut (VOLITEĽNÉ)
    /// Označuje, či sa vozidlá s osobnými autami môžu naložiť na túto jazdu
    /// </summary>
    public CarsAllowed? CarsAllowed { get; set; }
}