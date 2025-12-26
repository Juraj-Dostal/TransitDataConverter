namespace TDCLibrary.GtfsModel;

/// <summary>
/// Predstavuje preklad textu (translations.txt - VOLITEĽNÝ súbor).
/// Poskytuje preklady názvov a iných textových polí do rôznych jazykov.
/// </summary>
public class Translation
{
    /// <summary>
    /// Názov tabuľky obsahujúcej pole na preklad (POVINNÉ)
    /// Napr. "agency", "stops", "routes"
    /// </summary>
    public string TableName { get; set; } = string.Empty;
    
    /// <summary>
    /// Názov poľa, ktoré sa má preložiť (POVINNÉ)
    /// </summary>
    public string FieldName { get; set; } = string.Empty;
    
    /// <summary>
    /// Jazyk tohto prekladu (POVINNÉ)
    /// ISO 639-1 alebo 639-2 kód
    /// </summary>
    public string Language { get; set; } = string.Empty;
    
    /// <summary>
    /// Preložená hodnota (POVINNÉ)
    /// </summary>
    public string TranslationText { get; set; } = string.Empty;
    
    /// <summary>
    /// Hodnota z record_id identifikujúca záznam (PODMIENEČNE POVINNÉ)
    /// </summary>
    public string? RecordId { get; set; }
    
    /// <summary>
    /// Pomáha prekladači s kontextom (VOLITEĽNÉ)
    /// </summary>
    public string? RecordSubId { get; set; }
    
    /// <summary>
    /// Definuje hodnotu, ktorá sa má preložiť (PODMIENEČNE POVINNÉ)
    /// </summary>
    public string? FieldValue { get; set; }
}
