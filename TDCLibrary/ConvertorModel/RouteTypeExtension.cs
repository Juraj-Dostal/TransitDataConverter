using TDCLibrary.GtfsModel.Enums;
using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary.ConvertorModel;

public class RouteTypeExtension
{
    public static DopravnyProstriedok ToDopravnyProstriedok(RouteType routeType)
    {
        return routeType switch
        {
            RouteType.Tram => DopravnyProstriedok.Elektricka,
            RouteType.Subway => DopravnyProstriedok.Metro,
            RouteType.Rail => throw new NotSupportedException("Enum dopravneho prostriedku Vlak nie je podporovaný v JDF formáte."),
            RouteType.Bus => DopravnyProstriedok.Autobus,
            RouteType.Ferry => DopravnyProstriedok.Trajekt,
            RouteType.CableTram => DopravnyProstriedok.LanovaDraha,
            RouteType.AerialLift => DopravnyProstriedok.LanovaDraha,
            RouteType.Funicular => DopravnyProstriedok.LanovaDraha,
            RouteType.Trolleybus => DopravnyProstriedok.Trolejbus,
            RouteType.Monorail => throw new NotSupportedException("Enum dopravneho prostriedku Monorail nie je podporovaný v JDF formáte."),
            _ => throw new NotSupportedException("Neznáma hodnota RouteType.")
        };
    }
    
    public static RouteType FromDopravnyProstriedok(DopravnyProstriedok dopravnyProstriedok)
    {
        return dopravnyProstriedok switch
        {
            DopravnyProstriedok.Autobus => RouteType.Bus,
            DopravnyProstriedok.Elektricka => RouteType.Tram,
            DopravnyProstriedok.LanovaDraha => RouteType.AerialLift,
            DopravnyProstriedok.Metro => RouteType.Subway,
            DopravnyProstriedok.Trajekt => RouteType.Ferry,
            DopravnyProstriedok.Trolejbus => RouteType.Trolleybus,
            _ => throw new NotSupportedException("Neznáma hodnota RouteType.")
        };
    }
}