namespace TDCLibrary.GtfsModel.Enums;

/// <summary>
/// Typ výnimky v kalendári
/// </summary>
public enum ExceptionType
{
    /// <summary>
    /// Služba pridaná - Service added
    /// Služba bola pridaná pre tento dátum (funguje v deň, ktorý by inak bol neaktívny)
    /// </summary>
    ServiceAdded = 1,
    
    /// <summary>
    /// Služba odstránená - Service removed
    /// Služba bola odstránená pre tento dátum (nefunguje v deň, ktorý by inak bol aktívny)
    /// </summary>
    ServiceRemoved = 2
}
