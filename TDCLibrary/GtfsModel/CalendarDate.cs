using TDCLibrary.GtfsModel.Enums;

namespace TDCLibrary.GtfsModel;

/// <summary>
/// Predstavuje výnimky kalendára služby (calendar_dates.txt - PODMIENEČNE POVINNÝ súbor).
/// Definuje výnimky pre služby definované v calendar.txt, ako sú sviatky alebo špeciálne dni.
/// Tento súbor alebo calendar.txt musí byť prítomný.
/// </summary>
public class CalendarDate
{
    /// <summary>
    /// Identifikátor služby (POVINNÉ)
    /// </summary>
    public string ServiceId { get; set; } = string.Empty;
    
    /// <summary>
    /// Dátum výnimky (POVINNÉ)
    /// Formát: YYYYMMDD
    /// </summary>
    public string Date { get; set; } = string.Empty;
    
    /// <summary>
    /// Typ výnimky (POVINNÉ)
    /// </summary>
    public ExceptionType ExceptionType { get; set; }
}
