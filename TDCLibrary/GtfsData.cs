using TDCLibrary.GtfsModel;

namespace TDCLibrary.GtsfModel;

/// <summary>
/// Kontajner pre všetky GTFS dáta načítané zo súborov.
/// Obsahuje všetky dostupné údaje z GTFS datasetu.
/// </summary>
public class GtfsData
{
    /// <summary>
    /// Zoznam dopravných agentúr (agency.txt - POVINNÝ)
    /// </summary>
    public List<Agency> Agencies { get; set; } = new();
    
    /// <summary>
    /// Zoznam zastávok (stops.txt - POVINNÝ)
    /// </summary>
    public List<Stop> Stops { get; set; } = new();
    
    /// <summary>
    /// Zoznam trás (routes.txt - POVINNÝ)
    /// </summary>
    public List<Route> Routes { get; set; } = new();
    
    /// <summary>
    /// Zoznam jázd (trips.txt - POVINNÝ)
    /// </summary>
    public List<Trip> Trips { get; set; } = new();
    
    /// <summary>
    /// Zoznam časov zastávok (stop_times.txt - POVINNÝ)
    /// </summary>
    public List<StopTime> StopTimes { get; set; } = new();
    
    /// <summary>
    /// Zoznam kalendárov služieb (calendar.txt - PODMIENEČNE POVINNÝ)
    /// </summary>
    public List<Calendar> Calendars { get; set; } = new();
    
    /// <summary>
    /// Zoznam výnimiek kalendára (calendar_dates.txt - PODMIENEČNE POVINNÝ)
    /// </summary>
    public List<CalendarDate> CalendarDates { get; set; } = new();
    
    /// <summary>
    /// Zoznam cenových atribútov (fare_attributes.txt - VOLITEĽNÝ)
    /// </summary>
    public List<FareAttribute> FareAttributes { get; set; } = new();
    
    /// <summary>
    /// Zoznam cenových pravidiel (fare_rules.txt - VOLITEĽNÝ)
    /// </summary>
    public List<FareRule> FareRules { get; set; } = new();
    
    /// <summary>
    /// Zoznam tvarov trás (shapes.txt - VOLITEĽNÝ)
    /// </summary>
    public List<Shape> Shapes { get; set; } = new();
    
    /// <summary>
    /// Zoznam frekvencií (frequencies.txt - VOLITEĽNÝ)
    /// </summary>
    public List<Frequency> Frequencies { get; set; } = new();
    
    /// <summary>
    /// Zoznam prestupov (transfers.txt - VOLITEĽNÝ)
    /// </summary>
    public List<Transfer> Transfers { get; set; } = new();
    
    /// <summary>
    /// Zoznam chodníkov (pathways.txt - VOLITEĽNÝ)
    /// </summary>
    public List<Pathway> Pathways { get; set; } = new();
    
    /// <summary>
    /// Zoznam úrovní (levels.txt - VOLITEĽNÝ)
    /// </summary>
    public List<Level> Levels { get; set; } = new();
    
    /// <summary>
    /// Informácie o datasete (feed_info.txt - VOLITEĽNÝ)
    /// </summary>
    public FeedInfo? FeedInfo { get; set; }
    
    /// <summary>
    /// Zoznam prekladov (translations.txt - VOLITEĽNÝ)
    /// </summary>
    public List<Translation> Translations { get; set; } = new();
    
    /// <summary>
    /// Zoznam atribútov (attributions.txt - VOLITEĽNÝ)
    /// </summary>
    public List<Attribution> Attributions { get; set; } = new();
}
