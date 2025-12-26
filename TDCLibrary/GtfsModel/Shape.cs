namespace TDCLibrary.GtfsModel;

/// <summary>
/// Predstavuje tvar trasy (shapes.txt - VOLITEĽNÝ súbor).
/// Definuje presnú geografickú cestu, po ktorej vozidlo cestuje.
/// Užitočné pre zobrazenie trasy na mape.
/// </summary>
public class Shape
{
    /// <summary>
    /// Jedinečný identifikátor tvaru (POVINNÉ)
    /// </summary>
    public string ShapeId { get; set; } = string.Empty;
    
    /// <summary>
    /// Zemepisná šírka bodu tvaru (POVINNÉ)
    /// WGS84 formát
    /// </summary>
    public double ShapePtLat { get; set; }
    
    /// <summary>
    /// Zemepisná dĺžka bodu tvaru (POVINNÉ)
    /// WGS84 formát
    /// </summary>
    public double ShapePtLon { get; set; }
    
    /// <summary>
    /// Poradie bodu v rámci tvaru (POVINNÉ)
    /// Začína od 0 a zvyšuje sa
    /// </summary>
    public int ShapePtSequence { get; set; }
    
    /// <summary>
    /// Skutočná vzdialenosť prejdená od prvého bodu (VOLITEĽNÉ)
    /// V metroch alebo iných jednotkách
    /// </summary>
    public double? ShapeDistTraveled { get; set; }
}
