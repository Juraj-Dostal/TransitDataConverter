using TDCLibrary.ConvertorModel;
using TDCLibrary.GtfsModel.Enums;
using TDCLibrary.GtsfModel;
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
        // jdfData.Oznacniky = ConvertOznacniky(gtfsData);
        jdfData.Dopravci = ConvertDopravci(gtfsData);
        jdfData.Linky = ConvertLinky(gtfsData);
        jdfData.Zaslinky = ConvertZaslinky(gtfsData);
        jdfData.Spoje = ConvertSpoje(gtfsData);
        jdfData.Zasspoje = ConvertZasspoje(gtfsData);
        jdfData.PevnyKod = ConvertPevnykod(gtfsData);
        jdfData.Caskody = ConvertCasKodu(gtfsData);
        
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
            // var id = StopExtension.SplitStopId(stop.StopId);
            //var nazov = StopExtension.SplitStopName(stop.StopName);
            
            var zastavka = new Zastavky
            {
                Cislo = int.Parse(stop.StopId), //id.CisloZastavky,
                NazovObce = stop.StopName, //nazov.Obec, 
                //BlizkeMiesto = nazov.BlizkeMiesto,
                Stat = "SK", 
            };
            
            // Nastav pevný kód pre bezbariérovosť, ak je dostupný
            if (stop.WheelchairBoarding == WheelchairAccessibility.Accessible)
            {
                zastavka.PevneKody[0] = PevnyKodOznacenie.Bezbarierovost;
            }
            
            zastavkyList.Add(zastavka);
            //zastavkyIds.Add(id.CisloZastavky);
        }
        
        return zastavkyList;
    }

    public static List<Oznacniky> ConvertOznacniky(GtfsData gtfsData)
    {
        var oznacnikyList = new List<Oznacniky>();
        
        foreach (var stop in gtfsData.Stops)
        {
            var id = int.Parse(stop.StopId); //StopExtension.SplitStopId(stop.StopId);
            
            var oznacniky = new Oznacniky()
            {
                CisloZastavky = id,//.CisloZastavky,
                //KodOznacniku = id.KodOznaciku,
                Nazov = stop.StopName,
                Stanoviste = stop.PlatformCode
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
                Sidlo = "",
                TelefonSidlo = agency.AgencyPhone ?? "000000000", 
                TelefonDispecink = null,
                TelefonInformace = null,
                Fax = null,
                Email = agency.AgencyEmail,
                Web = agency.AgencyUrl,
                RozlisenieDopravcu = 0001, //int.Parse(agency.AgencyId),
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
                ObjizdkovyJR = true, 
                SeskupenieSpojov = false, 
                PouzitieOznacnikov = true, 
                Rezerva = null,
                CisloLicencie = RouteExtension.ToRouteId(route.RouteId).ToString(),
                PlatnostLicencieOd = null,
                PlatnostLicencieDo = null,
                PlatnostJROd = DateOnly.MinValue.ToString("ddMMyyyy"), // Todo: zada uzivatel 
                PlatnostJRDo = null,
                RozlisenieDopravcu = 1,
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
            var zaslinka = new Zaslinky
            {
                CisloLinky =  RouteExtension.FindRouteIdFromTripId(gtfsData.Trips, stopTime.TripId),
                CisloTarifni = stopTime.StopSequence,
                TarifniPasmo = "100", 
                CisloZastavky = int.Parse(stopTime.StopId), //StopExtension.SplitStopId(stopTime.StopId).CisloZastavky,
                PriemernaDoba = null,
                RozlisenieLinky = 1, 
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
                Cislo = int.Parse(trip.TripId.Replace("_", "")), 
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
                var stop = gtfsData.Stops.FirstOrDefault(s => s.StopId == stopTime.StopId);
                var id = StopExtension.SplitStopId(stop.StopId);
                int tarifniCislo = isOppositeDirection ? totalStops - i : i + 1;

                var zasspoj = new Zasspoje
                {
                    CisloLinky = RouteExtension.FindRouteIdFromTripId(gtfsData.Trips, stopTime.TripId),
                    CisloSpoje = int.Parse(stopTime.TripId.Replace("_", "")),
                    CisloTarifni = tarifniCislo,
                    CisloZastavky = int.Parse(stop.StopId),//id.CisloZastavky,
                    //KodOznacniku = id.KodOznaciku,
                    CisloStanoviste = stop.PlatformCode,
                    Kilometry = null,
                    CasPrichodu = Zasspoje.ConvertTime(stopTime.ArrivalTime),
                    CasOdchodu = Zasspoje.ConvertTime(stopTime.DepartureTime),
                    RozlisenieLinky = 1,
                };

                zasspojeList.Add(zasspoj);
            }
        }
        
        return zasspojeList;
    }

    public static List<Caskody> ConvertCasKodu(GtfsData gtfsData)
    {
        var caskodyList = new List<Caskody>();
        
        int casovyKodCounter = 1;
        var serviceToSpoje = gtfsData.Trips
            .OrderBy(t => t.ServiceId)
            .GroupBy(t => t.ServiceId)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Pre každý serviceId získaj časové segmenty
        foreach (var serviceEntry in serviceToSpoje)
        {
            var serviceId = serviceEntry.Key;
            var trips = serviceEntry.Value;
            
            // Získaj časové segmenty pre tento service
            var timeSegments = CalendarExtension.GetTimeSegmentsForService(serviceId, gtfsData);
            
            if (timeSegments.Count == 0)
            {
                Console.WriteLine($"WARNING: No time segments found for serviceId: {serviceId}");
                continue;
            }

            Console.WriteLine($"ServiceId {serviceId}: Found {timeSegments.Count} time segment(s)");
            
            // Vypis segmenty
            for (int i = 0; i < timeSegments.Count; i++)
            {
                var seg = timeSegments[i];
                Console.WriteLine($"  Segment {i+1}: {seg.StartDate:dd.MM.yyyy} - {seg.EndDate:dd.MM.yyyy}, PevneKody: {string.Join(",", seg.PevneKody)}");
            }
            
            // Skontroluj, či existujú calendar_dates pre tento service
            var hasCalendarDateRestrictions = gtfsData.CalendarDates
                .Any(cd => cd.ServiceId == serviceId && cd.ExceptionType == ExceptionType.ServiceAdded);
            
            Console.WriteLine($"  hasCalendarDateRestrictions: {hasCalendarDateRestrictions}");
            
            // Ak je iba jeden segment a NEMÁ calendar_dates obmedzenia, nepotrebujeme časové kódy
            if (timeSegments.Count == 1 && !hasCalendarDateRestrictions)
            {
                var segment = timeSegments[0];
                Console.WriteLine($"  Single segment without calendar_dates: {segment.StartDate:dd.MM.yyyy} - {segment.EndDate:dd.MM.yyyy}");
                // Pevné kódy sú už nastavené v Spoje, nie je potrebný časový kód
                continue;
            }
            
            // Vytvor Caskody pre tento service (buď má viacero segmentov, alebo má calendar_dates obmedzenia)
            Console.WriteLine($"  Creating Caskody for {trips.Count} trip(s) with {timeSegments.Count} segment(s)");
            
            // Pre každý trip v tomto service vytvor časové kódy pre každý segment
            foreach (var trip in trips)
            {
                Console.WriteLine($"  Processing trip: {trip.TripId}");
                int segmentNumber = 1;
                foreach (var segment in timeSegments)
                {
                    Console.WriteLine($"    Processing segment {segmentNumber}: {segment.StartDate:dd.MM.yyyy} - {segment.EndDate:dd.MM.yyyy}, PevneKody count: {segment.PevneKody.Count}");
                    
                    // Vytvor časový kód pre tento segment
                    var caskod = new Caskody
                    {
                        CisloLinky = RouteExtension.ToRouteId(trip.RouteId),
                        CisloSpoje = int.Parse(trip.TripId.Replace("_", "")),
                        Cislo = casovyKodCounter,
                        Oznacenie = 10 + (casovyKodCounter % 90), // Označenia 10-99
                        Typ = TypCasKod.Jede,
                        DatumOd = segment.StartDate.ToString("ddMMyyyy"),
                        DatumDo = segment.EndDate.ToString("ddMMyyyy"),
                        Poznamka = $"Obdobie {segmentNumber}",
                        RozlisenieLinky = 1
                    };
                    
                    caskodyList.Add(caskod);
                    Console.WriteLine($"    Created Caskod #{casovyKodCounter} for trip {trip.TripId}: " +
                                    $"{segment.StartDate:dd.MM.yyyy} - {segment.EndDate:dd.MM.yyyy}, " +
                                    $"PevneKody: {string.Join(", ", segment.PevneKody)}");
                    
                    casovyKodCounter++;
                    segmentNumber++;
                }
            }
        }

        Console.WriteLine($"Total Caskody created: {caskodyList.Count}");
        return caskodyList;
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