using TDCLibrary.ConvertorModel;
using TDCLibrary.GtsfModel;
using TDCLibrary.GtsfModel.Enums;
using TDCLibrary.JdfModel;
using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary;

public class Gtfs2Jdf
{
    public static JdfData Convert(GtfsData gtsfData)
    {
        var jdfData = new JdfData();
        
        jdfData.VerzeJDF = ConvertVerzeJDF(gtsfData);
        jdfData.Zastavky = ConvertZastavky(gtsfData);
        jdfData.Dopravci = ConvertDopravci(gtsfData);
        jdfData.Linky = ConvertLinky(gtsfData);
        jdfData.Zaslinky = ConvertZaslinky(gtsfData);
        jdfData.Spoje = ConvertSpoje(gtsfData);
        jdfData.Zasspoje = ConvertZasspoje(gtsfData);
        jdfData.PevnyKod = ConvertPevnykod(gtsfData);
        
        return jdfData;
    }
    
    public static VerzeJDF ConvertVerzeJDF(GtfsData gtsfData)
    {
        var verzeJDF = new VerzeJDF
        {
            VerziaJDF = "1.0",
            DatumVyrobyDat = System.DateTime.Now.ToString("ddMMyyyy")
        };
        return verzeJDF;
    }
    
    public static List<Zastavky> ConvertZastavky(GtfsData gtsfData)
    {
        var zastavkyList = new List<Zastavky>();
        
        var zastavkyIds = new HashSet<string>();
        
        foreach (var stop in gtsfData.Stops)
        {
            if (zastavkyIds.Contains(stop.StopId))
            {
                continue;
            }
            
            var zastavka = new Zastavky
            {
                Cislo = int.Parse(stop.StopCode),
                NazovObce = "Zilina", // Todo: vyriesit ze uzivatel zada
                BlizkeMiesto = stop.StopName,
                Stat = "SK", // Todo: vyriesit ze uzivatel zada
                PevnyKod1 = stop.WheelchairBoarding == WheelchairAccessibility.Accessible ? PevnyKodExtensions.DajCislo(PevnyKodOznacenie.Bezbarierovost) : null, // Todo: nejako urobit ze uzivatel zada
            };
            zastavkyList.Add(zastavka);
            zastavkyIds.Add(stop.StopId);
        }
        
        return zastavkyList;
    }
    
    public static List<Dopravci> ConvertDopravci(GtfsData gtsfData)
    {
        var dopravciList = new List<Dopravci>();
        
        foreach (var agency in gtsfData.Agencies)
        {
            var dopravca = new Dopravci
            {
                IC = agency.AgencyId?.ToString() ?? "0",
                DIC = null,
                ObchodnéMeno = agency.AgencyName,
                DruhFirmy = DruhFirmy.PravnickaOsoba, // Todo: doriesit
                MenoFyzOsoby = null,
                Sidlo = "Adresa dopravcu", // Todo: doriesit
                TelefonSidlo = agency.AgencyPhone ?? "000000000", // Todo: doriesit
                TelefonDispecink = null,
                TelefonInformace = null,
                Fax = null,
                Email = agency.AgencyEmail,
                Web = agency.AgencyUrl,
                RozlisenieDopravcu = int.Parse(agency.AgencyId),
            };
            dopravciList.Add(dopravca);
        }
        
        return dopravciList;
    }
    
    // Todo
    public static List<Linky> ConvertLinky(GtfsData gtsfData)
    {
        var linkyList = new List<Linky>();
        
        foreach (var route in gtsfData.Routes)
        {
            var linka = new Linky
            {
                Cislo = int.Parse(route.RouteId),
                Nazov = route.RouteId.ToString(),
                IcDopravce = route.AgencyId?.ToString() ?? "0",
                Typ = TypLinky.Mestska, // Todo: doriesit
                DopravnyProstriedok = RouteTypeExtension.ToDopravnyProstriedok(route.RouteType),
                ObjizdkovyJR = true,
                SeskupenieSpojov = false,
                PouzitieOznacnikov = true,
                CisloLicencie = route.RouteId.ToString(),
                PlatnostLicencieOd = null,
                PlatnostLicencieDo = null,
                PlatnostJROd = DateOnly.MinValue.ToString("ddMMyyyy"),
                PlatnostJRDo = null,
                RozlisenieDopravcu = int.Parse(route.AgencyId) ,
                RozlisenieLinky = int.Parse(route.RouteId)
            };
            linkyList.Add(linka);
        }
        
        return linkyList;
    }
    
    public static List<Zaslinky> ConvertZaslinky(GtfsData gtsfData)
    {
        var zaslinkyList = new List<Zaslinky>();

        foreach (var stopTime in gtsfData.StopTimes)
        {
            var zaslinka = new Zaslinky
            {
                CisloLinky = int.Parse(stopTime.TripId), // Todo: doriesit
                CisloTarifni = stopTime.StopSequence,
                TarifniPasmo = null,
                CisloZastavky = int.Parse(stopTime.StopId),
                PriemernaDoba = null,
                RozlisenieLinky = int.Parse(stopTime.TripId), // Todo: doriesit
            };
        }
        
        return zaslinkyList;
    }
    
    // Todo: doriesit pevne kody kedy ide podla kalendata
    public static List<Spoje> ConvertSpoje(GtfsData gtsfData)
    {
        var spojeList = new List<Spoje>();

        foreach (var trip in gtsfData.Trips)
        {
            var spoj = new Spoje
            {
                CisloLinky = int.Parse(trip.RouteId),
                Cislo = int.Parse(trip.TripId),
                KodSkupinySpoju = null, 
                RozliseniLinky = int.Parse(trip.RouteId)
            };
            spojeList.Add(spoj);
        }
        
        return spojeList;
    }
    
    public static List<Zasspoje> ConvertZasspoje(GtfsData gtsfData)
    {
        var zasspojeList = new List<Zasspoje>();

        foreach (var stopTime in gtsfData.StopTimes)
        {
            var zasspoj = new Zasspoje
            {
                CisloLinky = int.Parse(stopTime.TripId), // Todo: doriesit
                CisloSpoje = int.Parse(stopTime.TripId),
                CisloTarifni = stopTime.StopSequence,
                CisloZastavky = int.Parse(stopTime.StopId),
                KodOznacniku = null,
                CisloStanoviste = null,
                PevnyKod1 = null,
                PevnyKod2 = null,
                Kilometry = null,
                CasPrichodu = Zasspoje.ConvertTime(stopTime.ArrivalTime),
                CasOdchodu = Zasspoje.ConvertTime(stopTime.DepartureTime),
                RozlisenieLinky = int.Parse(stopTime.TripId), // Todo: doriesit
            };
            zasspojeList.Add(zasspoj);
        }
        
        return zasspojeList;
    }
    
    public static List<Pevnykod> ConvertPevnykod(GtfsData gtsfData)
    {
        var pevnykodList = new List<Pevnykod>();
        
        var pevnykody = Enum.GetValues(typeof(PevnyKodOznacenie));
        foreach (PevnyKodOznacenie kod in pevnykody)
        {
            var pevnykod = new Pevnykod
            {
                Cislo = PevnyKodExtensions.DajCislo(kod),
                Oznacenie = PevnyKodExtensions.ZiskajZnak(kod),
                Rezerva = PevnyKodExtensions.ZiskajNazov(kod)
            };
            pevnykodList.Add(pevnykod);
        }
        
        return pevnykodList;
    }
}