namespace TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Typ rozvrhu pre frekvencie (frequencies.txt)
/// Označuje, či sú časy v frekvencii presné alebo približné
/// </summary>
public enum FrequencyExactTimes
{
    /// <summary>
    /// Nepresný rozvrh - Frequency-based service
    /// Služba nie je založená na presnom rozvrhu, časy sú približné
    /// </summary>
    FrequencyBased = 0,
    
    /// <summary>
    /// Presný rozvrh - Schedule-based service
    /// Služba je založená na presnom rozvrhu, časy sú presné
    /// </summary>
    ScheduleBased = 1
}
