namespace TDCLibrary.GtfsModel;

/// <summary>
/// Predstavuje kalendár služby (calendar.txt - PODMIENEČNE POVINNÝ súbor).
/// Určuje dátumy, kedy je služba dostupná pre jednu alebo viac trás pomocou týždenného rozvrhu.
/// Tento súbor alebo calendar_dates.txt musí byť prítomný.
/// </summary>
public class Calendar
{
    /// <summary>
    /// Jedinečný identifikátor služby (POVINNÉ)
    /// </summary>
    public string ServiceId { get; set; } = string.Empty;
    
    /// <summary>
    /// Či služba funguje v pondelok (POVINNÉ)
    /// true - áno, false - nie
    /// </summary>
    public bool Monday { get; set; }
    
    /// <summary>
    /// Či služba funguje v utorok (POVINNÉ)
    /// true - áno, false - nie
    /// </summary>
    public bool Tuesday { get; set; }
    
    /// <summary>
    /// Či služba funguje v stredu (POVINNÉ)
    /// true - áno, false - nie
    /// </summary>
    public bool Wednesday { get; set; }
    
    /// <summary>
    /// Či služba funguje vo štvrtok (POVINNÉ)
    /// true - áno, false - nie
    /// </summary>
    public bool Thursday { get; set; }
    
    /// <summary>
    /// Či služba funguje v piatok (POVINNÉ)
    /// true - áno, false - nie
    /// </summary>
    public bool Friday { get; set; }
    
    /// <summary>
    /// Či služba funguje v sobotu (POVINNÉ)
    /// true - áno, false - nie
    /// </summary>
    public bool Saturday { get; set; }
    
    /// <summary>
    /// Či služba funguje v nedeľu (POVINNÉ)
    /// true - áno, false - nie
    /// </summary>
    public bool Sunday { get; set; }
    
    /// <summary>
    /// Dátum začiatku služby (POVINNÉ)
    /// Formát: YYYYMMDD
    /// </summary>
    public string StartDate { get; set; } = string.Empty;
    
    /// <summary>
    /// Dátum ukončenia služby (POVINNÉ)
    /// Formát: YYYYMMDD
    /// </summary>
    public string EndDate { get; set; } = string.Empty;
}
