namespace TDCLibrary.GtsfModel;

/// <summary>
/// Predstavuje úroveň v rámci stanice (levels.txt - VOLITEĽNÝ súbor).
/// Popisuje úrovne v stanici. Užitočné pre viacúrovňové stanice.
/// </summary>
public class Level
{
    /// <summary>
    /// Jedinečný identifikátor úrovne (POVINNÉ)
    /// </summary>
    public string LevelId { get; set; } = string.Empty;
    
    /// <summary>
    /// Numerický index úrovne (POVINNÉ)
    /// Prízemie = 0, vyššie úrovne = kladné čísla, nižšie = záporné
    /// </summary>
    public double LevelIndex { get; set; }
    
    /// <summary>
    /// Názov úrovne (VOLITEĽNÉ)
    /// Napr. "Prízemie", "Mezzanine", "Platforma"
    /// </summary>
    public string? LevelName { get; set; }
}
