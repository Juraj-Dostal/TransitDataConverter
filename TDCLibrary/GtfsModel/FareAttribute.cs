using TDCLibrary.GtfsModel.Enums;

namespace TDCLibrary.GtfsModel;

/// <summary>
/// Predstavuje cenové pravidlá (fare_attributes.txt - VOLITEĽNÝ súbor).
/// Definuje informácie o cestovnom pre trasy organizácie.
/// </summary>
public class FareAttribute
{
    /// <summary>
    /// Jedinečný identifikátor cestovného (POVINNÉ)
    /// </summary>
    public string FareId { get; set; } = string.Empty;
    
    /// <summary>
    /// Cena cestovného (POVINNÉ)
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Typ meny (POVINNÉ)
    /// ISO 4217 kód (napr. "EUR", "USD")
    /// </summary>
    public string CurrencyType { get; set; } = string.Empty;
    
    /// <summary>
    /// Kedy sa platí (POVINNÉ)
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }
    
    /// <summary>
    /// Počet prestupov (POVINNÉ)
    /// </summary>
    public TransfersAllowed? Transfers { get; set; }
    
    /// <summary>
    /// ID agentúry (VOLITEĽNÉ)
    /// </summary>
    public string? AgencyId { get; set; }
    
    /// <summary>
    /// Dĺžka platnosti lístka v sekundách (VOLITEĽNÉ)
    /// </summary>
    public int? TransferDuration { get; set; }
}
