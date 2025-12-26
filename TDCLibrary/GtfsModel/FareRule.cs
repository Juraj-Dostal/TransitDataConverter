namespace TDCLibrary.GtfsModel;

/// <summary>
/// Predstavuje pravidlá pre aplikovanie cestovného (fare_rules.txt - VOLITEĽNÝ súbor).
/// Priradí cestovné k trasám pomocou spojenia origin_id, destination_id a contains_id.
/// </summary>
public class FareRule
{
    /// <summary>
    /// Identifikátor cestovného (POVINNÉ)
    /// </summary>
    public string FareId { get; set; } = string.Empty;
    
    /// <summary>
    /// Identifikátor trasy (VOLITEĽNÉ)
    /// Ak je viacero trás s rovnakými cenovými atribútmi, vytvoriť záznam pre každú
    /// </summary>
    public string? RouteId { get; set; }
    
    /// <summary>
    /// Identifikátor počiatočnej zóny (VOLITEĽNÉ)
    /// </summary>
    public string? OriginId { get; set; }
    
    /// <summary>
    /// Identifikátor cieľovej zóny (VOLITEĽNÉ)
    /// </summary>
    public string? DestinationId { get; set; }
    
    /// <summary>
    /// Identifikátor zóny, cez ktorú musí cestujúci prejsť (VOLITEĽNÉ)
    /// </summary>
    public string? ContainsId { get; set; }
}
