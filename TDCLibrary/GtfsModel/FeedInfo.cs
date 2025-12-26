namespace TDCLibrary.GtfsModel;

/// <summary>
/// Predstavuje informácie o zdroji datasetu (feed_info.txt - VOLITEĽNÝ súbor).
/// Poskytuje metadata o samotnom GTFS datasete.
/// </summary>
public class FeedInfo
{
    /// <summary>
    /// Názov vydavateľa datasetu (POVINNÉ)
    /// </summary>
    public string FeedPublisherName { get; set; } = string.Empty;
    
    /// <summary>
    /// URL webovej stránky vydavateľa (POVINNÉ)
    /// </summary>
    public string FeedPublisherUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Primárny jazyk datasetu (POVINNÉ)
    /// ISO 639-1 kód (napr. "sk", "en")
    /// </summary>
    public string FeedLang { get; set; } = string.Empty;
    
    /// <summary>
    /// Predvolený jazyk (VOLITEĽNÉ)
    /// </summary>
    public string? DefaultLang { get; set; }
    
    /// <summary>
    /// Dátum začiatku platnosti datasetu (VOLITEĽNÉ)
    /// Formát: YYYYMMDD
    /// </summary>
    public string? FeedStartDate { get; set; }
    
    /// <summary>
    /// Dátum konca platnosti datasetu (VOLITEĽNÉ)
    /// Formát: YYYYMMDD
    /// </summary>
    public string? FeedEndDate { get; set; }
    
    /// <summary>
    /// Verzia datasetu (VOLITEĽNÉ)
    /// </summary>
    public string? FeedVersion { get; set; }
    
    /// <summary>
    /// Email kontakt (VOLITEĽNÉ)
    /// </summary>
    public string? FeedContactEmail { get; set; }
    
    /// <summary>
    /// URL kontaktu (VOLITEĽNÉ)
    /// </summary>
    public string? FeedContactUrl { get; set; }
}
