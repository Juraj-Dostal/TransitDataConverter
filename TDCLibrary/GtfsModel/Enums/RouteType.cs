namespace TDCLibrary.GtfsModel.Enums;

/// <summary>
/// Typ dopravnej trasy
/// </summary>
public enum RouteType
{
    /// <summary>
    /// Električky, tramvaje - Tram, Streetcar, Light rail
    /// Akýkoľvek ľahký koľajový systém alebo električka na úrovni ulice
    /// </summary>
    Tram = 0,
    
    /// <summary>
    /// Metro, podzemná dráha - Subway, Metro
    /// Akýkoľvek podzemný koľajový systém v mestskej oblasti
    /// </summary>
    Subway = 1,
    
    /// <summary>
    /// Vlak, železnica - Rail
    /// Používa sa pre medzimestskú alebo diaľkovú železničnú dopravu
    /// </summary>
    Rail = 2,
    
    /// <summary>
    /// Autobus - Bus
    /// Krátke a dlhé autobusové linky
    /// </summary>
    Bus = 3,
    
    /// <summary>
    /// Trajekt - Ferry
    /// Krátka a dlhá vodná doprava
    /// </summary>
    Ferry = 4,
    
    /// <summary>
    /// Lanovka - Cable tram
    /// Pouličná lanovka, kde kabíny sú ťahané podzemným lanom
    /// </summary>
    CableTram = 5,
    
    /// <summary>
    /// Visutá lanovka, kabínková lanovka - Aerial lift, Suspended cable car
    /// Lanová doprava kde kabíny visia na lanách (napr. gondola, kabínková lanovka)
    /// </summary>
    AerialLift = 6,
    
    /// <summary>
    /// Pozemná lanovka - Funicular
    /// Koľajový systém navrhnutý pre strmé svahy
    /// </summary>
    Funicular = 7,
    
    /// <summary>
    /// Trolejbus - Trolleybus
    /// Elektrický autobus napájaný z nadzemných káblov
    /// </summary>
    Trolleybus = 11,
    
    /// <summary>
    /// Jednokoľajka - Monorail
    /// Železnica, kde trať pozostáva z jednej koľaje
    /// </summary>
    Monorail = 12
}
