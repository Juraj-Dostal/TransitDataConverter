namespace TDCLibrary.GtsfModel;

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
    /// 1 - áno, 0 - nie
    /// </summary>
    public int Monday { get; set; }
    
    /// <summary>
    /// Či služba funguje v utorok (POVINNÉ)
    /// 1 - áno, 0 - nie
    /// </summary>
    public int Tuesday { get; set; }
    
    /// <summary>
    /// Či služba funguje v stredu (POVINNÉ)
    /// 1 - áno, 0 - nie
    /// </summary>
    public int Wednesday { get; set; }
    
    /// <summary>
    /// Či služba funguje vo štvrtok (POVINNÉ)
    /// 1 - áno, 0 - nie
    /// </summary>
    public int Thursday { get; set; }
    
    /// <summary>
    /// Či služba funguje v piatok (POVINNÉ)
    /// 1 - áno, 0 - nie
    /// </summary>
    public int Friday { get; set; }
    
    /// <summary>
    /// Či služba funguje v sobotu (POVINNÉ)
    /// 1 - áno, 0 - nie
    /// </summary>
    public int Saturday { get; set; }
    
    /// <summary>
    /// Či služba funguje v nedeľu (POVINNÉ)
    /// 1 - áno, 0 - nie
    /// </summary>
    public int Sunday { get; set; }
    
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
