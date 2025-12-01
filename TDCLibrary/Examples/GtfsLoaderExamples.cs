// using TDCLibrary;
// using TDCLibrary.GtsfModel;
//
// namespace TDCLibrary.Examples;
//
// /// <summary>
// /// Príklady použitia GTFS Načítavača
// /// </summary>
// public class GtfsLoaderExamples
// {
//     /// <summary>
//     /// Základný príklad - načítanie všetkých dát
//     /// </summary>
//     public static void PrikladZakladneNacitanie()
//     {
//         // Cesta k GTFS adresáru
//         var gtfsPath = "/cesta/k/gtfs/adresaru";
//         
//         // Vytvorte načítavač
//         var loader = new GtfsLoader(gtfsPath);
//         
//         // Načítajte všetky dáta
//         var gtfsData = loader.LoadAll();
//         
//         // Zobrazenie základných štatistík
//         Console.WriteLine($"Načítané GTFS dáta:");
//         Console.WriteLine($"  Agentúry: {gtfsData.Agencies.Count}");
//         Console.WriteLine($"  Zastávky: {gtfsData.Stops.Count}");
//         Console.WriteLine($"  Trasy: {gtfsData.Routes.Count}");
//         Console.WriteLine($"  Jazdy: {gtfsData.Trips.Count}");
//         Console.WriteLine($"  Časy zastávok: {gtfsData.StopTimes.Count}");
//     }
//     
//     /// <summary>
//     /// Príklad - načítanie konkrétnych súborov
//     /// </summary>
//     public static void PrikladSelektivneNacitanie()
//     {
//         var gtfsPath = "/cesta/k/gtfs/adresaru";
//         var loader = new GtfsLoader(gtfsPath);
//         
//         // Načítajte len konkrétne súbory
//         var agencies = loader.LoadAgencies();
//         var routes = loader.LoadRoutes();
//         var stops = loader.LoadStops();
//         
//         Console.WriteLine($"Načítané {agencies.Count} agentúr");
//         Console.WriteLine($"Načítané {routes.Count} trás");
//         Console.WriteLine($"Načítané {stops.Count} zastávok");
//     }
//     
//     /// <summary>
//     /// Príklad - zobrazenie informácií o trasách
//     /// </summary>
//     public static void PrikladZobrazenieTras(string gtfsPath)
//     {
//         var loader = new GtfsLoader(gtfsPath);
//         var gtfsData = loader.LoadAll();
//         
//         Console.WriteLine("\n=== ZOZNAM TRÁS ===");
//         foreach (var route in gtfsData.Routes)
//         {
//             var typNazov = ZiskajTypTrasy(route.RouteType);
//             Console.WriteLine($"{route.RouteShortName} - {route.RouteLongName}");
//             Console.WriteLine($"  Typ: {typNazov}");
//             if (!string.IsNullOrEmpty(route.RouteColor))
//             {
//                 Console.WriteLine($"  Farba: #{route.RouteColor}");
//             }
//             
//             // Počet jázd pre túto trasu
//             var pocetJazd = gtfsData.Trips.Count(t => t.RouteId == route.RouteId);
//             Console.WriteLine($"  Počet jázd: {pocetJazd}");
//             Console.WriteLine();
//         }
//     }
//     
//     /// <summary>
//     /// Príklad - zobrazenie zastávok pre konkrétnu jazdu
//     /// </summary>
//     public static void PrikladZobrazenieCasovRozvrhu(string gtfsPath, string tripId)
//     {
//         var loader = new GtfsLoader(gtfsPath);
//         var gtfsData = loader.LoadAll();
//         
//         // Nájdite jazdu
//         var trip = gtfsData.Trips.FirstOrDefault(t => t.TripId == tripId);
//         if (trip == null)
//         {
//             Console.WriteLine($"Jazda s ID '{tripId}' nebola nájdená.");
//             return;
//         }
//         
//         // Nájdite trasu
//         var route = gtfsData.Routes.FirstOrDefault(r => r.RouteId == trip.RouteId);
//         
//         Console.WriteLine($"\n=== ČASOVÝ ROZVRH ===");
//         Console.WriteLine($"Trasa: {route?.RouteShortName} - {route?.RouteLongName}");
//         Console.WriteLine($"Smer: {trip.TripHeadsign}");
//         Console.WriteLine();
//         
//         // Získajte časy zastávok
//         var stopTimes = gtfsData.StopTimes
//             .Where(st => st.TripId == tripId)
//             .OrderBy(st => st.StopSequence)
//             .ToList();
//         
//         Console.WriteLine("Čas príchodu | Čas odchodu | Zastávka");
//         Console.WriteLine("-------------|-------------|----------");
//         
//         foreach (var stopTime in stopTimes)
//         {
//             var stop = gtfsData.Stops.FirstOrDefault(s => s.StopId == stopTime.StopId);
//             Console.WriteLine($"{stopTime.ArrivalTime,12} | {stopTime.DepartureTime,11} | {stop?.StopName}");
//         }
//     }
//     
//     /// <summary>
//     /// Príklad - vyhľadávanie zastávok v okolí
//     /// </summary>
//     public static void PrikladVyhladanieZastavokVOkoli(string gtfsPath, double lat, double lon, double radiusKm)
//     {
//         var loader = new GtfsLoader(gtfsPath);
//         var gtfsData = loader.LoadAll();
//         
//         Console.WriteLine($"\n=== ZASTÁVKY V OKOLÍ {lat}, {lon} ===");
//         Console.WriteLine($"Polomer: {radiusKm} km\n");
//         
//         var zastavkyVOkoli = new List<(Stop stop, double vzdialenost)>();
//         
//         foreach (var stop in gtfsData.Stops)
//         {
//             if (stop.StopLat.HasValue && stop.StopLon.HasValue)
//             {
//                 var vzdialenost = VypocitajVzdialenost(lat, lon, stop.StopLat.Value, stop.StopLon.Value);
//                 if (vzdialenost <= radiusKm)
//                 {
//                     zastavkyVOkoli.Add((stop, vzdialenost));
//                 }
//             }
//         }
//         
//         // Zoraď podle vzdialenosti
//         zastavkyVOkoli = zastavkyVOkoli.OrderBy(x => x.vzdialenost).ToList();
//         
//         foreach (var (stop, vzdialenost) in zastavkyVOkoli)
//         {
//             Console.WriteLine($"{stop.StopName}");
//             Console.WriteLine($"  Vzdialenosť: {vzdialenost:F2} km");
//             Console.WriteLine($"  Súradnice: {stop.StopLat}, {stop.StopLon}");
//             Console.WriteLine();
//         }
//     }
//     
//     /// <summary>
//     /// Príklad - kontrola služieb v konkrétny deň
//     /// </summary>
//     public static void PrikladKontrolaSluzby(string gtfsPath, DateTime datum)
//     {
//         var loader = new GtfsLoader(gtfsPath);
//         var gtfsData = loader.LoadAll();
//         
//         var datumString = datum.ToString("yyyyMMdd");
//         var denVTyzdni = (int)datum.DayOfWeek;
//         
//         Console.WriteLine($"\n=== SLUŽBY AKTÍVNE V DENI {datum:dd.MM.yyyy} ({datum:dddd}) ===\n");
//         
//         var aktivneSluzby = new HashSet<string>();
//         
//         // Kontrola kalendára
//         foreach (var calendar in gtfsData.Calendars)
//         {
//             if (string.Compare(datumString, calendar.StartDate) >= 0 &&
//                 string.Compare(datumString, calendar.EndDate) <= 0)
//             {
//                 var jeAktivny = denVTyzdni switch
//                 {
//                     0 => calendar.Sunday,   // Nedeľa
//                     1 => calendar.Monday,   // Pondelok
//                     2 => calendar.Tuesday,  // Utorok
//                     3 => calendar.Wednesday,// Streda
//                     4 => calendar.Thursday, // Štvrtok
//                     5 => calendar.Friday,   // Piatok
//                     6 => calendar.Saturday, // Sobota
//                     _ => false
//                 };
//                 
//                 if (jeAktivny)
//                 {
//                     aktivneSluzby.Add(calendar.ServiceId);
//                 }
//             }
//         }
//         
//         // Kontrola výnimiek
//         foreach (var calendarDate in gtfsData.CalendarDates)
//         {
//             if (calendarDate.Date == datumString)
//             {
//                 if (calendarDate.ExceptionType == GtsfModel.Enums.ExceptionType.ServiceAdded)
//                 {
//                     aktivneSluzby.Add(calendarDate.ServiceId);
//                 }
//                 else if (calendarDate.ExceptionType == GtsfModel.Enums.ExceptionType.ServiceRemoved)
//                 {
//                     aktivneSluzby.Remove(calendarDate.ServiceId);
//                 }
//             }
//         }
//         
//         Console.WriteLine($"Počet aktívnych služieb: {aktivneSluzby.Count}");
//         
//         // Zobrazenie trás s aktívnymi jazdami
//         var trasySJazdami = gtfsData.Trips
//             .Where(t => aktivneSluzby.Contains(t.ServiceId))
//             .Select(t => t.RouteId)
//             .Distinct()
//             .ToList();
//         
//         Console.WriteLine($"\nTrasy s aktívnymi jazdami:");
//         foreach (var routeId in trasySJazdami)
//         {
//             var route = gtfsData.Routes.FirstOrDefault(r => r.RouteId == routeId);
//             if (route != null)
//             {
//                 var pocetJazd = gtfsData.Trips.Count(t => 
//                     t.RouteId == routeId && aktivneSluzby.Contains(t.ServiceId));
//                 Console.WriteLine($"  {route.RouteShortName} - {route.RouteLongName} ({pocetJazd} jázd)");
//             }
//         }
//     }
//     
//     // Pomocné metódy
//     
//     private static string ZiskajTypTrasy(GtsfModel.Enums.RouteType routeType)
//     {
//         return routeType switch
//         {
//             GtsfModel.Enums.RouteType.Tram => "Električky / Tram",
//             GtsfModel.Enums.RouteType.Subway => "Metro / Subway",
//             GtsfModel.Enums.RouteType.Rail => "Vlak / Rail",
//             GtsfModel.Enums.RouteType.Bus => "Autobus / Bus",
//             GtsfModel.Enums.RouteType.Ferry => "Trajekt / Ferry",
//             GtsfModel.Enums.RouteType.CableTram => "Lanovka / Cable tram",
//             GtsfModel.Enums.RouteType.AerialLift => "Visutá lanovka / Aerial lift",
//             GtsfModel.Enums.RouteType.Funicular => "Pozemná lanovka / Funicular",
//             GtsfModel.Enums.RouteType.Trolleybus => "Trolejbus / Trolleybus",
//             GtsfModel.Enums.RouteType.Monorail => "Jednokoľajka / Monorail",
//             _ => "Neznámy typ"
//         };
//     }
//     
//     private static double VypocitajVzdialenost(double lat1, double lon1, double lat2, double lon2)
//     {
//         // Haversine vzorec pre výpočet vzdialenosti medzi dvoma GPS bodmi
//         const double R = 6371; // Polomer Zeme v km
//         
//         var dLat = ToRadians(lat2 - lat1);
//         var dLon = ToRadians(lon2 - lon1);
//         
//         var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
//                 Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
//                 Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
//         
//         var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
//         
//         return R * c;
//     }
//     
//     private static double ToRadians(double degrees)
//     {
//         return degrees * Math.PI / 180.0;
//     }
// }
