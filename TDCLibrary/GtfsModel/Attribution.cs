namespace TDCLibrary.GtfsModel;

/// <summary>
/// Predstavuje atribúty (attributions.txt - VOLITEĽNÝ súbor).
/// Definuje priradenia údajov, ktoré sa môžu vzťahovať na dataset.
/// </summary>
public class Attribution
{
    /// <summary>
    /// Jedinečný identifikátor atribútu (VOLITEĽNÉ)
    /// </summary>
    public string? AttributionId { get; set; }
    
    /// <summary>
    /// Názov organizácie (VOLITEĽNÉ)
    /// </summary>
    public string? AgencyId { get; set; }
    
    /// <summary>
    /// Identifikátor trasy (VOLITEĽNÉ)
    /// </summary>
    public string? RouteId { get; set; }
    
    /// <summary>
    /// Identifikátor jazdy (VOLITEĽNÉ)
    /// </summary>
    public string? TripId { get; set; }
    
    /// <summary>
    /// Názov organizácie zodpovednej za dataset (POVINNÉ)
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;
    
    /// <summary>
    /// Či je atribúcia producent (VOLITEĽNÉ)
    /// 0 - nie, 1 - áno
    /// </summary>
    public int? IsProducer { get; set; }
    
    /// <summary>
    /// Či je atribúcia prevádzkovateľ (VOLITEĽNÉ)
    /// 0 - nie, 1 - áno
    /// </summary>
    public int? IsOperator { get; set; }
    
    /// <summary>
    /// Či je atribúcia poskytovateľ (VOLITEĽNÉ)
    /// 0 - nie, 1 - áno
    /// </summary>
    public int? IsAuthority { get; set; }
    
    /// <summary>
    /// URL organizácie (VOLITEĽNÉ)
    /// </summary>
    public string? AttributionUrl { get; set; }
    
    /// <summary>
    /// Email kontakt organizácie (VOLITEĽNÉ)
    /// </summary>
    public string? AttributionEmail { get; set; }
    
    /// <summary>
    /// Telefónne číslo organizácie (VOLITEĽNÉ)
    /// </summary>
    public string? AttributionPhone { get; set; }
}
