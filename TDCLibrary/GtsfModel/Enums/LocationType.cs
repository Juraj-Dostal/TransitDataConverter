namespace TDCLibrary.GtsfModel.Enums;

/// <summary>
/// Typ miesta zastávky
/// </summary>
public enum LocationType
{
    /// <summary>
    /// Zastávka alebo platforma - Stop or Platform
    /// Miesto, kde cestujúci nastupujú alebo vystupujú z dopravného prostriedku
    /// </summary>
    StopOrPlatform = 0,
    
    /// <summary>
    /// Stanica - Station
    /// Fyzická štruktúra alebo oblasť obsahujúca jednu alebo viac platforiem
    /// </summary>
    Station = 1,
    
    /// <summary>
    /// Vstup alebo výstup stanice - Entrance or Exit
    /// Miesto, kde cestujúci môžu vstúpiť alebo vystúpiť zo stanice
    /// </summary>
    EntranceOrExit = 2,
    
    /// <summary>
    /// Generický uzol - Generic Node
    /// Miesto v rámci stanice, ktoré nie je žiadny z vyššie uvedených typov
    /// </summary>
    GenericNode = 3,
    
    /// <summary>
    /// Oblasť nástupu - Boarding Area
    /// Špecifické miesto na platforme, kde cestujúci môžu nastúpiť
    /// </summary>
    BoardingArea = 4
}
