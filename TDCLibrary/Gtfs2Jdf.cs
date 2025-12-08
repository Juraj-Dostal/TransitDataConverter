using TDCLibrary.ConvertorModel;
using TDCLibrary.GtsfModel;
using TDCLibrary.GtsfModel.Enums;
using TDCLibrary.JdfModel;
using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary;

public class Gtfs2Jdf
{
    public static JdfData Convert(GtfsData gtfsData)
    {
        var jdfData = new JdfData();
        
        jdfData.VerzeJDF = ConvertVerzeJDF(gtfsData);
        jdfData.Zastavky = ConvertZastavky(gtfsData);
        jdfData.Oznacniky = ConvertOznacniky(gtfsData);
        jdfData.Dopravci = ConvertDopravci(gtfsData);
        jdfData.Linky = ConvertLinky(gtfsData);
        jdfData.Zaslinky = ConvertZaslinky(gtfsData);
        jdfData.Spoje = ConvertSpoje(gtfsData);
        jdfData.Zasspoje = ConvertZasspoje(gtfsData);
        jdfData.PevnyKod = ConvertPevnykod(gtfsData);
        
        return jdfData;
    }
    
    public static VerzeJDF ConvertVerzeJDF(GtfsData gtfsData)
    {
        var verzeJDF = new VerzeJDF
        {
            VerziaJDF = "1.0",
            DatumVyrobyDat = System.DateTime.Now.ToString("ddMMyyyy")
        };
        return verzeJDF;
    }
    
    public static List<Zastavky> ConvertZastavky(GtfsData gtfsData)
    {
        var zastavkyList = new List<Zastavky>();
        var zastavkyIds = new HashSet<int>();
        
        foreach (var stop in gtfsData.Stops)
        {
            var id = StopExtension.SplitStopId(stop.StopId);
            
            if (zastavkyIds.Contains(id.CisloZastavky))
            {
                continue;
            }
            
            var zastavka = new Zastavky
            {
                Cislo = id.CisloZastavky,
                NazovObce = "BA", // Todo: vyriesit ze uzivatel zada
                BlizkeMiesto = stop.StopName,
                Stat = "SK", // Todo: vyriesit ze uzivatel zada
            };
            
            // Nastav pevný kód pre bezbariérovosť, ak je dostupný
            if (stop.WheelchairBoarding == WheelchairAccessibility.Accessible)
            {
                zastavka.PevneKody[0] = PevnyKodOznacenie.Bezbarierovost;
            }
            
            zastavkyList.Add(zastavka);
            zastavkyIds.Add(id.CisloZastavky);
        }
        
        return zastavkyList;
    }

    public static List<Oznacniky> ConvertOznacniky(GtfsData gtfsData)
    {
        var oznacnikyList = new List<Oznacniky>();
        
        foreach (var stop in gtfsData.Stops)
        {
            var id = StopExtension.SplitStopId(stop.StopId);
            
            var oznacniky = new Oznacniky()
            {
                CisloZastavky = id.CisloZastavky,
                KodOznacniku = id.KodOznaciku,
                Nazov = stop.StopName,
            };
            
            oznacnikyList.Add(oznacniky);
        }

        return oznacnikyList;
    }
    
    public static List<Dopravci> ConvertDopravci(GtfsData gtfsData)
    {
        var dopravciList = new List<Dopravci>();
        
        foreach (var agency in gtfsData.Agencies)
        {
            var dopravca = new Dopravci
            {
                IC = agency.AgencyId?.ToString() ?? "0",
                DIC = null,
                ObchodnéMeno = agency.AgencyName,
                DruhFirmy = DruhFirmy.PravnickaOsoba, 
                MenoFyzOsoby = null,
                Sidlo = "Adresa dopravcu",
                TelefonSidlo = agency.AgencyPhone ?? "000000000", 
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
    public static List<Linky> ConvertLinky(GtfsData gtfsData)
    {
        var linkyList = new List<Linky>();
        
        foreach (var route in gtfsData.Routes)
        {
            var linka = new Linky
            {
                Cislo = RouteExtension.ToRouteId(route.RouteId),
                Nazov = route.RouteLongName,
                IcDopravce = route.AgencyId?.ToString() ?? "0",
                Typ = TypLinky.VnitrastatniVnitrokrajska, 
                DopravnyProstriedok = RouteTypeExtension.ToDopravnyProstriedok(route.RouteType), 
                ObjizdkovyJR = true, // ToDo: Otazne
                SeskupenieSpojov = false, // ToDo: Otazne
                PouzitieOznacnikov = true, // ToDo: Otazne
                Rezerva = null,
                CisloLicencie = RouteExtension.ToRouteId(route.RouteId).ToString(),
                PlatnostLicencieOd = null,
                PlatnostLicencieDo = null,
                PlatnostJROd = DateOnly.MinValue.ToString("ddMMyyyy"), // Todo: zada uzivatel 
                PlatnostJRDo = null,
                RozlisenieDopravcu = int.Parse(route.AgencyId),
                RozlisenieLinky = 1
            };
            linkyList.Add(linka);
        }
        
        return linkyList;
    }
    
    public static List<Zaslinky> ConvertZaslinky(GtfsData gtfsData)
    {
        var zaslinkyList = new List<Zaslinky>();

        foreach (var stopTime in gtfsData.StopTimes)
        {
            if (!int.TryParse(stopTime.StopId, out int result))
            {
                continue;
            }
            
            var zaslinka = new Zaslinky
            {
                CisloLinky =  RouteExtension.FindRouteIdFromTripId(gtfsData.Trips, stopTime.TripId),
                CisloTarifni = stopTime.StopSequence,
                TarifniPasmo = "100", // Todo: tarifne info
                CisloZastavky = StopExtension.SplitStopId(stopTime.StopId).CisloZastavky,
                PriemernaDoba = null,
                RozlisenieLinky = int.Parse(stopTime.TripId), // Todo: zada uzivatel
            };

            zaslinka.PevneKody[0] = PevnyKodOznacenie.ZastavkaNaZiadost;
            
            zaslinkyList.Add(zaslinka);
        }
        
        return zaslinkyList;
    }
    
    // Todo: doriesit pevne kody kedy ide podla kalendata
    public static List<Spoje> ConvertSpoje(GtfsData gtfsData)
    {
        var spojeList = new List<Spoje>();

        foreach (var trip in gtfsData.Trips)
        {
            var spoj = new Spoje
            {
                CisloLinky = RouteExtension.ToRouteId(trip.RouteId),
                Cislo = int.Parse(trip.TripId), 
                KodSkupinySpoju = 0, 
                RozliseniLinky = 1
            };

            var pevneKody = CalendarExtension.MapServiceIdToPevnyKod(trip.ServiceId, gtfsData);
            
            // Priradenie pevných kódov do poľa (max 10)
            for (int i = 0; i < Math.Min(pevneKody.Count, 10); i++)
            {
                spoj.PevneKody[i] = pevneKody[i];
            }
            
            if (trip.WheelchairAccessible == WheelchairAccessibility.Accessible)
            {
                spoj.PevneKody[pevneKody.Count] = PevnyKodOznacenie.Bezbarierovost;
            }
            
            spojeList.Add(spoj);
        }
        
        return spojeList;
    }

    
    public static List<Zasspoje> ConvertZasspoje(GtfsData gtfsData)
    {
        var zasspojeList = new List<Zasspoje>();
        
        // Zoskupí sa stopTimes podľa tripId
        var stopTimesByTrip = gtfsData.StopTimes
            .Where(st => int.TryParse(st.StopId, out _))
            .GroupBy(st => st.TripId)
            .ToDictionary(g => g.Key, g => g.OrderBy(st => st.StopSequence).ToList());

        foreach (var tripGroup in stopTimesByTrip)
        {
            var tripId = tripGroup.Key;
            var stopTimes = tripGroup.Value;

            var route = RouteExtension.FindRouteFromTripId(gtfsData.Trips, tripId);
            if (route == null)
            {
                throw new ArgumentException("Route does not exists");
            }

            int totalStops = stopTimes.Count;
            bool isOppositeDirection = route.DirectionId == DirectionId.OppositeDirection;

            for (int i = 0; i < stopTimes.Count; i++)
            {
                var stopTime = stopTimes[i];
                int tarifniCislo = isOppositeDirection ? totalStops - i : i + 1;

                var zasspoj = new Zasspoje
                {
                    CisloLinky = RouteExtension.FindRouteIdFromTripId(gtfsData.Trips, stopTime.TripId),
                    CisloSpoje = int.Parse(stopTime.TripId),
                    CisloTarifni = tarifniCislo,
                    CisloZastavky = StopExtension.SplitStopId(stopTime.StopId).CisloZastavky,
                    KodOznacniku = null,
                    CisloStanoviste = null,
                    Kilometry = null,
                    CasPrichodu = Zasspoje.ConvertTime(stopTime.ArrivalTime),
                    CasOdchodu = Zasspoje.ConvertTime(stopTime.DepartureTime),
                    RozlisenieLinky = int.Parse(stopTime.TripId),
                };

                zasspojeList.Add(zasspoj);
            }
        }
        
        return zasspojeList;
    }
    
    
    public static List<Pevnykod> ConvertPevnykod(GtfsData gtfsData)
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