# TDCLibrary - GTFS Načítavač

Knižnica pre načítanie a spracovanie GTFS (General Transit Feed Specification) dát v C#.

## Popis

TDCLibrary poskytuje komplexný načítavač pre všetky štandardné GTFS súbory podľa oficiálnej špecifikácie z [gtfs.org](https://gtfs.org/documentation/schedule/reference/).

Knižnica podporuje:
- ✅ Všetky **povinné** GTFS súbory (agency, stops, routes, trips, stop_times)
- ✅ Všetky **podmienečne povinné** súbory (calendar, calendar_dates)
- ✅ Všetky **voliteľné** súbory (fare_attributes, fare_rules, shapes, frequencies, transfers, pathways, levels, feed_info, translations, attributions)
- ✅ Automatické spracovanie chýbajúcich voliteľných súborov
- ✅ Podpora neúplných dát (chýbajúce voliteľné polia)
- ✅ UTF-8 kódovanie pre správne spracovanie diakritiky

## Štruktúra projektu

```
TDCLibrary/
├── Models/                      # GTFS modely
│   ├── Agency.cs               # Dopravné agentúry
│   ├── Stop.cs                 # Zastávky a stanice
│   ├── Route.cs                # Trasy
│   ├── Trip.cs                 # Jazdy
│   ├── StopTime.cs             # Časy zastávok
│   ├── Calendar.cs             # Kalendár služieb
│   ├── CalendarDate.cs         # Výnimky kalendára
│   ├── FareAttribute.cs        # Cenové atribúty
│   ├── FareRule.cs             # Cenové pravidlá
│   ├── Shape.cs                # Tvary trás
│   ├── Frequency.cs            # Frekvencie
│   ├── Transfer.cs             # Prestupy
│   ├── Pathway.cs              # Chodníky v staniciach
│   ├── Level.cs                # Úrovne v staniciach
│   ├── FeedInfo.cs             # Informácie o datasete
│   ├── Translation.cs          # Preklady
│   ├── Attribution.cs          # Atribúty
│   └── GtfsData.cs             # Kontajner pre všetky dáta
├── GtfsLoader.cs               # Hlavný načítavač
└── GtfsLoaderExamples.cs       # Príklady použitia
```

## Použitie

### Základné načítanie

```csharp
using TDCLibrary;

// Vytvorte načítavač s cestou k GTFS adresáru
var loader = new GtfsLoader("/cesta/k/gtfs/adresaru");

// Načítajte všetky dostupné dáta
var gtfsData = loader.LoadAll();

// Prístup k dátam
Console.WriteLine($"Počet agentúr: {gtfsData.Agencies.Count}");
Console.WriteLine($"Počet zastávok: {gtfsData.Stops.Count}");
Console.WriteLine($"Počet trás: {gtfsData.Routes.Count}");
```

### Selektívne načítanie

```csharp
var loader = new GtfsLoader("/cesta/k/gtfs/adresaru");

// Načítajte len konkrétne súbory
var agencies = loader.LoadAgencies();
var routes = loader.LoadRoutes();
var stops = loader.LoadStops();
```

### Práca s dátami

```csharp
var gtfsData = loader.LoadAll();

// Nájdite konkrétnu trasu
var route = gtfsData.Routes.FirstOrDefault(r => r.RouteShortName == "32");

// Získajte jazdy pre túto trasu
var trips = gtfsData.Trips.Where(t => t.RouteId == route.RouteId).ToList();

// Pre každú jazdu zobrazte zastávky
foreach (var trip in trips)
{
    var stopTimes = gtfsData.StopTimes
        .Where(st => st.TripId == trip.TripId)
        .OrderBy(st => st.StopSequence)
        .ToList();
    
    foreach (var stopTime in stopTimes)
    {
        var stop = gtfsData.Stops.FirstOrDefault(s => s.StopId == stopTime.StopId);
        Console.WriteLine($"{stopTime.ArrivalTime} - {stop?.StopName}");
    }
}
```

## Dokumentácia

Kompletná slovenská dokumentácia je dostupná v súbore [GTFS_DOKUMENTACIA.md](GTFS_DOKUMENTACIA.md), ktorá obsahuje:

- Detailný popis každého GTFS súboru
- Vysvetlenie všetkých polí
- Praktické príklady použitia
- Informácie o povinnosti jednotlivých súborov a polí

## Príklady

Súbor `GtfsLoaderExamples.cs` obsahuje praktické príklady:

1. **Základné načítanie** - načítanie všetkých dát
2. **Selektívne načítanie** - načítanie konkrétnych súborov
3. **Zobrazenie trás** - výpis všetkých trás s detailmi
4. **Časový rozvrh** - zobrazenie zastávok pre konkrétnu jazdu
5. **Vyhľadávanie v okolí** - nájdenie zastávok v okolí GPS súradníc
6. **Kontrola služby** - zistenie aktívnych služieb v konkrétny deň

## Požiadavky

- .NET 8.0 alebo vyššie
- C# 12
- CsvHelper 33.1.0 (automaticky nainštalované cez NuGet)

## GTFS Špecifikácia

Táto knižnica je plne kompatibilná so špecifikáciou GTFS dostupnou na:
https://gtfs.org/documentation/schedule/reference/

## Autor

Vytvorené pre projekt TransitDataConverter

## Licencia

Použitie podľa potrieb projektu
