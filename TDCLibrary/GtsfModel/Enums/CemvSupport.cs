namespace TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Podpora bezkontaktných platobných kariet cEMV (contactless EMV)
/// Označuje, či cestujúci môžu použiť cEMV karty ako platobné médium pre jazdy asociované s touto agentúrou
/// </summary>
public enum CemvSupport
{
    /// <summary>
    /// Žiadna informácia o cEMV - No cEMV information
    /// Nie je k dispozícii informácia o podpore cEMV pre jazdy asociované s touto agentúrou
    /// </summary>
    NoInformation = 0,
    
    /// <summary>
    /// cEMV karty sú podporované - cEMVs supported
    /// Cestujúci môžu použiť cEMV karty ako platobné médium pre jazdy asociované s touto agentúrou
    /// </summary>
    Supported = 1,
    
    /// <summary>
    /// cEMV karty nie sú podporované - cEMVs not supported
    /// cEMV karty nie sú podporované ako platobné médium pre jazdy asociované s touto agentúrou
    /// </summary>
    NotSupported = 2
}
