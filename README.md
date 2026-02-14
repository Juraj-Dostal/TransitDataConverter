# Transit Data Converter (TDC)

**Knižnica a GUI aplikácia pre spracovanie, zobrazenie a konverziu údajov o verejnej doprave vo formátoch GTFS a JDF.**

---

## 📋 Obsah

- [O projekte](#-o-projekte)
- [Funkcie](#-funkcie)
- [Použité technológie](#-použité-technológie)
- [Štruktúra projektu](#-štruktúra-projektu)
- [Inštalácia a spustenie](#-inštalácia-a-spustenie)
- [GUI Aplikácia](#-gui-aplikácia)
- [GTFS Model](#-gtfs-model)
- [JDF Model](#-jdf-model)
- [Konverzia GTFS → JDF](#-konverzia-gtfs--jdf)
- [Použitie knižnice](#-použitie-knižnice)
- [Dokumentácia](#-dokumentácia)

---

## 📖 O projekte

**Transit Data Converter** je nástroj určený pre prácu s dátami verejnej dopravy. Projekt sa skladá z dvoch hlavných častí:

1. **TDCLibrary** - Core knižnica pre načítanie, spracovanie, konverziu a zápis GTFS a JDF dát
2. **TDCGui** - Grafické používateľské rozhraie pre vizualizáciu a editáciu dát

Projekt umožňuje pracovať s medzinárodným štandardom **GTFS** (General Transit Feed Specification) a slovenským formátom **JDF** (Jednotný Dátový Formát), ktorý sa používa pre evidenciu cestovných poriadkov verejnej dopravy na Slovensku.

---

## ✨ Funkcie

### Knižnica (TDCLibrary)
- ✅ **Načítanie GTFS** - Kompletný parser pre všetky štandardné GTFS súbory
- ✅ **Zápis GTFS** - Export dát späť do GTFS formátu
- ✅ **Načítanie JDF** - Parser pre slovenský JDF formát
- ✅ **Zápis JDF** - Export dát do JDF formátu
- ✅ **Konverzia GTFS → JDF** - Automatická transformácia medzi formátmi
- ✅ **Validácia dát** - Kontrola povinných a nepovinných polí
- ✅ **Enumerácie** - Typovo bezpečné hodnoty (napr. RouteType, LocationType)
- ✅ **Podpora rozšírení** - CEMV support, cars_allowed a ďalšie

### GUI Aplikácia (TDCGui)
- 📂 **Načítanie dát** - Výber adresára s GTFS alebo JDF súbormi
- 📊 **Zobrazenie dát** - Prehľadné tabuľky pre všetky typy záznamov
- ✏️ **Editácia** - Priama úprava hodnôt v DataGrid komponentoch
- 💾 **Uloženie zmien** - Export upravených dát späť na disk
- 🔄 **Konverzia** - Transformácia GTFS dát do JDF formátu
- 🎨 **Moderné UI** - Cross-platform rozhranie pomocou Avalonia UI
- 📋 **Viacero pohľadov** - Prepínanie medzi rôznymi typmi dát (Agency, Routes, Stops, Trips, atď.)

---

## 🛠 Použité technológie

| Technológia | Účel | Verzia |
|------------|------|--------|
| **C# / .NET** | Programovací jazyk a platforma | .NET 8.0 |
| **Avalonia UI** | Cross-platform GUI framework | 11.x |
| **ReactiveUI** | MVVM framework pre reactive programovanie | - |
| **CsvHelper** | Parsovanie a generovanie CSV súborov | - |
| **LINQ** | Spracovanie a dotazovanie dát | - |

### Prečo tieto technológie?

- **.NET 8.0** - Moderná, výkonná a cross-platform platforma
- **Avalonia UI** - Umožňuje vytvárať natívne aplikácie pre Windows, Linux a macOS z jedného kódu
- **ReactiveUI** - Zjednodušuje implementáciu MVVM pattern a reaktívne UI
- **CsvHelper** - Robustná knižnica pre prácu s CSV súbormi (GTFS aj JDF používajú CSV)

---

## 📁 Štruktúra projektu

```
TransitDataConverter/
├── TDCLibrary/                          # 📚 Core knižnica
│   ├── GtsfModel/                       # GTFS dátové modely
│   │   ├── Agency.cs                    # Dopravné agentúry
│   │   ├── Stop.cs                      # Zastávky a stanice
│   │   ├── Route.cs                     # Linky/trasy
│   │   ├── Trip.cs                      # Jednotlivé jazdy
│   │   ├── StopTime.cs                  # Časy príchodov/odchodov
│   │   ├── Calendar.cs                  # Kalendár prevádzky
│   │   ├── CalendarDate.cs              # Výnimky v kalendári
│   │   ├── FareAttribute.cs             # Cenové atribúty
│   │   ├── FareRule.cs                  # Cenové pravidlá
│   │   ├── Shape.cs                     # Geometria trás
│   │   ├── Frequency.cs                 # Frekvenčné spoje
│   │   ├── Transfer.cs                  # Prestupy medzi spojmi
│   │   ├── Pathway.cs                   # Chodníky v staniciach
│   │   ├── Level.cs                     # Podlažia staníc
│   │   ├── FeedInfo.cs                  # Metadáta datasetu
│   │   ├── Translation.cs               # Preklady
│   │   ├── Attribution.cs               # Autorské práva
│   │   └── GtfsData.cs                  # Hlavný kontajner pre GTFS dáta
│   │
│   ├── JdfModel/                        # JDF dátové modely
│   │   ├── VerzeJDF.cs                  # Verzia JDF datasetu
│   │   ├── Dopravci.cs                  # Dopravcovia
│   │   ├── Zastavky.cs                  # Zastávky
│   │   ├── Linky.cs                     # Linky
│   │   ├── Zaslinky.cs                  # Zastávky na linkách
│   │   ├── Spoje.cs                     # Spoje
│   │   ├── Zasspoje.cs                  # Zastávky spojov
│   │   ├── Caskody.cs                   # Časové kódy
│   │   ├── Pevnykod.cs                  # Pevné kódy (dni prevádzky)
│   │   ├── Oznacniky.cs                 # Označníky
│   │   └── JdfData.cs                   # Hlavný kontajner pre JDF dáta
│   │
│   ├── ConvertorModel/                  # Pomocné triedy pre konverziu
│   │   ├── CalendarExtension.cs         # Pomocné metódy pre Calendar
│   │   ├── RouteExtension.cs            # Pomocné metódy pre Route
│   │   ├── RouteTypeExtension.cs        # Konverzia typov liniek
│   │   └── StopExtension.cs             # Pomocné metódy pre Stop
│   │
│   ├── Examples/                        # Ukážkové príklady použitia
│   │   ├── GtfsLoaderExamples.cs        # Príklady načítania GTFS
│   │   ├── JdfLoaderExamples.cs         # Príklady načítania JDF
│   │   ├── CemvSupportExample.cs        # Práca s CEMV
│   │   └── PevnyKodEnumPriklady.cs      # Práca s pevnými kódmi
│   │
│   ├── GtfsLoader.cs                    # 📥 GTFS načítavač
│   ├── GtfsWriter.cs                    # 📤 GTFS zapisovač
│   ├── JdfLoader.cs                     # 📥 JDF načítavač
│   ├── JdfWriter.cs                     # 📤 JDF zapisovač
│   ├── Gtfs2Jdf.cs                      # 🔄 Konvertor GTFS→JDF
│   └── TDCLibrary.csproj                # Project file
│
├── TDCGui/                              # 🖥 GUI aplikácia
│   ├── ViewModels/                      # MVVM ViewModely
│   │   ├── MainViewModel.cs             # Hlavný ViewModel
│   │   ├── JdfViewModel.cs              # ViewModel pre JDF okno
│   │   └── DataTypeItem.cs              # Pomocná trieda pre ComboBox
│   │
│   ├── Views/                           # XAML Views
│   │   ├── GtfsViews/                   # Views pre GTFS dáta
│   │   │   ├── AgencyDataControl.axaml  # Zobrazenie agentúr
│   │   │   ├── RoutesView.axaml         # Zobrazenie liniek
│   │   │   ├── StopsView.axaml          # Zobrazenie zastávok
│   │   │   ├── TripsView.axaml          # Zobrazenie jázd
│   │   │   ├── StopTimesView.axaml      # Zobrazenie časov
│   │   │   ├── CalendarView.axaml       # Zobrazenie kalendára
│   │   │   ├── CalendarDatesView.axaml  # Zobrazenie výnimiek
│   │   │   ├── FareAttributesView.axaml # Zobrazenie cien
│   │   │   ├── FareRulesView.axaml      # Zobrazenie cenových pravidiel
│   │   │   ├── ShapesView.axaml         # Zobrazenie tvarov trás
│   │   │   ├── FrequenciesView.axaml    # Zobrazenie frekvencií
│   │   │   ├── TransfersView.axaml      # Zobrazenie prestupov
│   │   │   ├── PathwaysView.axaml       # Zobrazenie chodníkov
│   │   │   ├── LevelsView.axaml         # Zobrazenie úrovní
│   │   │   ├── TranslationsView.axaml   # Zobrazenie prekladov
│   │   │   └── AttributionsView.axaml   # Zobrazenie atribúcií
│   │   │
│   │   └── JdfViews/                    # Views pre JDF dáta
│   │       ├── JdfVerzeView.axaml       # Zobrazenie verzie JDF
│   │       ├── JdfLinkyView.axaml       # Zobrazenie JDF liniek
│   │       ├── JdfZastavkyView.axaml    # Zobrazenie JDF zastávok
│   │       ├── JdfZaslinkyView.axaml    # Zobrazenie zastávok na linkách
│   │       ├── JdfZasspojeView.axaml    # Zobrazenie zastávok spojov
│   │       ├── JdfPevnykodView.axaml    # Zobrazenie pevných kódov
│   │       └── JdfOznacnikyView.axaml   # Zobrazenie označníkov
│   │
│   ├── MainWindow.axaml                 # Hlavné okno aplikácie
│   ├── MainWindow.axaml.cs              # Code-behind hlavného okna
│   ├── JdfWindow.axaml                  # Okno pre JDF dáta
│   ├── JdfWindow.axaml.cs               # Code-behind JDF okna
│   ├── App.axaml                        # Aplikačné štýly a zdroje
│   ├── Program.cs                       # Entry point
│   └── TDCGui.csproj                    # Project file
│
├── README.md                            # 📖 Táto dokumentácia
├── GTFS_DOKUMENTACIA.md                 # Detailná GTFS dokumentácia
├── JDF_FORMAT_DOKUMENTACIA.md           # Detailná JDF dokumentácia
├── JDF_FORMAT_PRIKLADY.md               # Príklady JDF súborov
└── TransitDataConverter.sln             # Visual Studio solution
```

---

## 🚀 Inštalácia a spustenie

### Požiadavky

- **.NET 8.0 SDK** alebo novší
- **IDE** (voliteľné): Visual Studio, Rider, alebo VS Code

### Inštalácia .NET SDK

**Windows:**
```bash
# Stiahnite z https://dotnet.microsoft.com/download
winget install Microsoft.DotNet.SDK.8
```

**Linux (Ubuntu/Debian):**
```bash
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0
```

**macOS:**
```bash
brew install dotnet-sdk
```

### Klonovanie projektu

```bash
git clone <repository-url>
cd TransitDataConverter
```

### Build projektu

```bash
# Build celého solution
dotnet build

# Alebo build len knižnice
cd TDCLibrary
dotnet build

# Alebo build len GUI
cd TDCGui
dotnet build
```

### Spustenie GUI aplikácie

```bash
cd TDCGui
dotnet run
```

Alternatívne môžete otvoriť `TransitDataConverter.sln` vo Visual Studio alebo Rider a spustiť projekt `TDCGui`.

---

## 🖥 GUI Aplikácia

### Popis rozhrania

GUI aplikácia poskytuje intuitívne rozhranie pre prácu s dopravnými dátami:

#### Hlavné okno (GTFS)

```
┌─────────────────────────────────────────────────────────────┐
│ [Vybrať priečinok] [Načítať] [Uložiť] [Konvertovať do JDF] │
├─────────────────────────────────────────────────────────────┤
│ Typ dát: [Agency ▼] ← ComboBox pre výber typu              │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ ID  │ Name        │ URL              │ Timezone       │ │
│  ├─────┼─────────────┼──────────────────┼────────────────┤ │
│  │ 1   │ City Bus    │ https://...      │ Europe/Prague  │ │
│  │ 2   │ Metro       │ https://...      │ Europe/Prague  │ │
│  │     │             │                  │                │ │
│  └───────────────────────────────────────────────────────┘ │
│                     DataGrid s dátami                       │
└─────────────────────────────────────────────────────────────┘
```

#### Funkcie

1. **Výber priečinka** - Dialógové okno pre výber adresára s GTFS súbormi
2. **Načítanie dát** - Automatické načítanie všetkých dostupných GTFS súborov
3. **Prepínanie typov** - ComboBox pre prepnutie medzi:
   - Agency (Agentúry)
   - Routes (Linky)
   - Stops (Zastávky)
   - Trips (Jazdy)
   - Stop Times (Časy zastávok)
   - Calendar (Kalendár)
   - Calendar Dates (Výnimky)
   - Fare Attributes (Cenové atribúty)
   - Fare Rules (Cenové pravidlá)
   - Shapes (Tvary trás)
   - Frequencies (Frekvencie)
   - Transfers (Prestupy)
   - Pathways (Chodníky)
   - Levels (Úrovne)
   - Translations (Preklady)
   - Attributions (Atribúcie)

4. **Editácia v DataGrid** - Dvojklikom na bunku môžete upraviť hodnotu
5. **Uloženie zmien** - Uloží upravené dáta späť do pôvodných CSV súborov
6. **Konverzia do JDF** - Otvorí nové okno s JDF dátami

#### JDF okno

Po konverzii sa otvorí nové okno s JDF dátami, kde môžete:
- Prezerať konvertované JDF záznamy
- Prepínať medzi JDF typmi (Verze, Dopravci, Zastavky, Linky, atď.)
- Upravovať hodnoty
- Uložiť JDF súbory na disk

---

## 📊 GTFS Model

GTFS (General Transit Feed Specification) je medzinárodný štandard pre dáta verejnej dopravy.

### Hlavné entity

#### 1. Agency (agency.txt) - Dopravná agentúra
```csharp
public class Agency
{
    public int AgencyId { get; set; }           // ID agentúry
    public string AgencyName { get; set; }      // Názov (napr. "Dopravný podnik")
    public string AgencyUrl { get; set; }       // Webová stránka
    public string AgencyTimezone { get; set; }  // Časové pásmo
    public string? AgencyLang { get; set; }     // Jazyk (napr. "sk")
    public string? AgencyPhone { get; set; }    // Telefón
    public CemvSupport? CemvSupport { get; set; } // Podpora bezkontaktných kariet
}
```

#### 2. Stop (stops.txt) - Zastávka
```csharp
public class Stop
{
    public int StopId { get; set; }              // Unikátne ID zastávky
    public string? StopCode { get; set; }        // Verejný kód
    public string? StopName { get; set; }        // Názov zastávky
    public double? StopLat { get; set; }         // GPS súradnica - šírka
    public double? StopLon { get; set; }         // GPS súradnica - dĺžka
    public LocationType? LocationType { get; set; } // Typ (zastávka/stanica/vchod)
    public int? ParentStation { get; set; }      // Rodičovská stanica
}
```

#### 3. Route (routes.txt) - Linka/Trasa
```csharp
public class Route
{
    public int RouteId { get; set; }             // ID linky
    public int? AgencyId { get; set; }           // ID agentúry
    public string? RouteShortName { get; set; }  // Krátky názov (napr. "7")
    public string? RouteLongName { get; set; }   // Dlhý názov
    public RouteType RouteType { get; set; }     // Typ (bus/tram/metro/rail)
    public string? RouteColor { get; set; }      // Farba linky (hex)
    public DirectionId? DirectionId { get; set; } // Smer (tam/späť)
}
```

#### 4. Trip (trips.txt) - Jazda
```csharp
public class Trip
{
    public int RouteId { get; set; }             // ID linky
    public int ServiceId { get; set; }           // ID služby (kalendár)
    public int TripId { get; set; }              // ID jazdy
    public string? TripHeadsign { get; set; }    // Cieľová tabuľa
    public DirectionId? DirectionId { get; set; } // Smer jazdy
    public CarsAllowed? CarsAllowed { get; set; } // Povolenie áut
}
```

#### 5. StopTime (stop_times.txt) - Čas zastávky
```csharp
public class StopTime
{
    public int TripId { get; set; }              // ID jazdy
    public int StopId { get; set; }              // ID zastávky
    public string? ArrivalTime { get; set; }     // Čas príchodu (HH:MM:SS)
    public string? DepartureTime { get; set; }   // Čas odchodu (HH:MM:SS)
    public int StopSequence { get; set; }        // Poradie zastávky
}
```

#### 6. Calendar (calendar.txt) - Kalendár prevádzky
```csharp
public class Calendar
{
    public int ServiceId { get; set; }           // ID služby
    public bool Monday { get; set; }             // Jazdí v pondelok?
    public bool Tuesday { get; set; }            // Jazdí v utorok?
    // ... ďalšie dni týždňa
    public string StartDate { get; set; }        // Začiatok platnosti (YYYYMMDD)
    public string EndDate { get; set; }          // Koniec platnosti (YYYYMMDD)
}
```

### Enumerácie

```csharp
// Typ linky
public enum RouteType
{
    Tram = 0,           // Električka
    Subway = 1,         // Metro
    Rail = 2,           // Vlak
    Bus = 3,            // Autobus
    Ferry = 4,          // Prievoz
    CableTram = 5,      // Lanová dráha
    AerialLift = 6,     // Lanovka
    Funicular = 7,      // Pozemná lanovka
    Trolleybus = 11,    // Trolejbus
    Monorail = 12       // Monorail
}

// Typ lokácie
public enum LocationType
{
    StopOrPlatform = 0, // Zastávka
    Station = 1,        // Stanica
    EntranceExit = 2,   // Vchod/východ
    GenericNode = 3,    // Uzol
    BoardingArea = 4    // Nástupná plocha
}

// Smer jazdy
public enum DirectionId
{
    DefaultDirection = 0,    // Východzí smer (tam)
    OppositeDirection = 1    // Opačný smer (späť)
}
```

**Viac informácií:** Pozri `GTFS_DOKUMENTACIA.md`

---

## 📋 JDF Model

JDF (Jednotný Dátový Formát) je slovenský štandard pre evidenciu cestovných poriadkov.

### Hlavné entity

#### 1. VerzeJDF (VerzeJDF.txt) - Verzia datasetu
```csharp
public class VerzeJDF
{
    public string VerziaJDF { get; set; }        // Verzia formátu (napr. "1.10")
    public int? CisloDU { get; set; }            // Číslo dopravnej jednotky
    public string? OkresKraj { get; set; }       // Okres/kraj
    public string DatumVyrobyDat { get; set; }   // Dátum vytvorenia (YYYYMMDD)
}
```

#### 2. Dopravci (Dopravci.txt) - Dopravca
```csharp
public class Dopravci
{
    public string IC { get; set; }               // IČO
    public string ObchodnéMeno { get; set; }     // Obchodné meno
    public DruhFirmy DruhFirmy { get; set; }     // Typ firmy (1=FO, 2=PO)
    public string Sidlo { get; set; }            // Sídlo
    public string TelefonSidlo { get; set; }     // Telefón
}
```

#### 3. Zastavky (Zastavky.txt) - Zastávka
```csharp
public class Zastavky
{
    public int Cislo { get; set; }               // Číslo zastávky (ID)
    public string NazovObce { get; set; }        // Názov obce
    public string? CastObce { get; set; }        // Časť obce
    public string Stat { get; set; }             // Štát (ISO kód)
    public PevnyKodOznacenie?[] PevneKody { get; set; } // Pevné kódy (6x)
}
```

#### 4. Linky (Linky.txt) - Linka
```csharp
public class Linky
{
    public int Cislo { get; set; }               // Číslo linky (6-miestne)
    public string Nazov { get; set; }            // Názov linky
    public string IcDopravce { get; set; }       // IČO dopravcu
    public TypLinky Typ { get; set; }            // Typ linky
    public DopravnyProstriedok DopravnyProstriedok { get; set; }
    public string PlatnostJROd { get; set; }     // Platnosť od (YYYYMMDD)
    public string PlatnostJRDo { get; set; }     // Platnosť do (YYYYMMDD)
}
```

#### 5. Spoje (Spoje.txt) - Spoj
```csharp
public class Spoje
{
    public int CisloLinky { get; set; }          // Číslo linky (6-miestne)
    public int Cislo { get; set; }               // Číslo spoja
    public PevnyKodOznacenie?[] PevneKody { get; set; } // Pevné kódy (10x)
    public int RozliseniLinky { get; set; }      // Rozlíšenie linky
}
```

#### 6. Zasspoje (Zasspoje.txt) - Zastávka spoja
```csharp
public class Zasspoje
{
    public int CisloLinky { get; set; }          // Číslo linky (6-miestne)
    public int CisloSpoje { get; set; }          // Číslo spoja
    public int CisloTarifni { get; set; }        // Tarifné číslo (poradie)
    public int CisloZastavky { get; set; }       // Číslo zastávky
    public string CasPrichodu { get; set; }      // Čas príchodu (HHMM)
    public string? CasOdchodu { get; set; }      // Čas odchodu (HHMM)
    public PevnyKodOznacenie?[] PevneKody { get; set; } // Pevné kódy (2x)
}
```

#### 7. Pevnykod (Pevnykod.txt) - Pevný kód
```csharp
public class Pevnykod
{
    public string Cislo { get; set; }            // 5-miestny kód
    public string Oznacenie { get; set; }        // Popis (napr. "X", "Po-Pi")
}
```

### Pevné kódy - Enumerácia

```csharp
public enum PevnyKodOznacenie
{
    Prac = 00001,        // Pracovný deň
    Neprac = 00002,      // Nepracovný deň
    Nedela = 00003,      // Nedeľa
    // ... celkovo 50+ kódov
}
```

**Príklad použitia:**
- `00001` - Jazdí v pracovné dni
- `00002` - Jazdí v nepracovné dni (sobota, nedeľa, sviatok)
- `00025` - Jazdí v pondelok až piatok okrem sviatkov

**Viac informácií:** Pozri `JDF_FORMAT_DOKUMENTACIA.md` a `JDF_FORMAT_PRIKLADY.md`

---

## 🔄 Konverzia GTFS → JDF

Konvertor `Gtfs2Jdf` automaticky transformuje GTFS dáta do JDF formátu:

### Mapovanie

| GTFS | JDF | Poznámka |
|------|-----|----------|
| Agency | Dopravci | 1:1 mapovanie |
| Stop | Zastavky | ID sa extrahuje z StopId |
| Route | Linky | RouteId → 6-miestne číslo |
| Trip | Spoje | TripId → číslo spoja |
| StopTime | Zasspoje | S časmi v HHMM formáte |
| Calendar + CalendarDate | Pevnykod (v Spojoch) | Dni prevádzky → pevné kódy |

### Príklad konverzie

**GTFS Route:**
```
route_id,route_short_name,route_type
10205,7,3
```

**JDF Linka:**
```
"010205","7","12345678",1,1,0,0,0,"","","","","20240101","20241231",1,1
```

Všimnite si:
- `10205` → `010205` (6-miestne číslo)
- `route_type=3` (Bus) → `DopravnyProstriedok=1` (Autobus)

### Použitie konvertora

```csharp
// Načítanie GTFS
var gtfsLoader = new GtfsLoader("./gtfs_data");
var gtfsData = gtfsLoader.LoadAllData();

// Konverzia
var jdfData = Gtfs2Jdf.Convert(gtfsData);

// Zápis JDF
var jdfWriter = new JdfWriter("./jdf_output");
jdfWriter.WriteAll(jdfData);
```

---

## 💻 Použitie knižnice

### Príklad 1: Načítanie GTFS dát

```csharp
using TDCLibrary;

// Vytvorenie loadera
var loader = new GtfsLoader("./cesta/k/gtfs/priecinku");

// Načítanie všetkých dát
var data = loader.LoadAllData();

// Prístup k dátam
Console.WriteLine($"Počet agentúr: {data.Agencies.Count}");
Console.WriteLine($"Počet zastávok: {data.Stops.Count}");
Console.WriteLine($"Počet liniek: {data.Routes.Count}");

// Iterácia cez zastávky
foreach (var stop in data.Stops)
{
    Console.WriteLine($"{stop.StopId}: {stop.StopName} ({stop.StopLat}, {stop.StopLon})");
}
```

### Príklad 2: Filtrovanie autobusových liniek

```csharp
var busRoutes = data.Routes
    .Where(r => r.RouteType == RouteType.Bus)
    .OrderBy(r => r.RouteShortName)
    .ToList();

foreach (var route in busRoutes)
{
    Console.WriteLine($"Autobus {route.RouteShortName}: {route.RouteLongName}");
}
```

### Príklad 3: Zápis GTFS dát

```csharp
// Úprava dát
data.Agencies[0].AgencyName = "Nový názov";

// Zápis späť
var writer = new GtfsWriter("./vystup");
writer.WriteAllData(data);
```

### Príklad 4: Načítanie a úprava JDF

```csharp
// Načítanie JDF
var jdfLoader = new JdfLoader("./jdf_priecinok");
var jdfData = jdfLoader.LoadAllData();

// Úprava
jdfData.Linky[0].Nazov = "Upravený názov";

// Zápis
var jdfWriter = new JdfWriter("./jdf_vystup");
jdfWriter.WriteAll(jdfData);
```

### Príklad 5: Konverzia s vlastnou logikou

```csharp
// Načítanie GTFS
var gtfsData = new GtfsLoader("./gtfs").LoadAllData();

// Konverzia základných entít
var jdfData = new JdfData
{
    Dopravci = Gtfs2Jdf.ConvertDopravci(gtfsData),
    Zastavky = Gtfs2Jdf.ConvertZastavky(gtfsData),
    Linky = Gtfs2Jdf.ConvertLinky(gtfsData),
    // ... ďalšie
};

// Vlastné úpravy
foreach (var linka in jdfData.Linky)
{
    linka.PlatnostJROd = "20240101";
    linka.PlatnostJRDo = "20241231";
}

// Zápis
new JdfWriter("./vystup").WriteAll(jdfData);
```

---

## 📚 Dokumentácia

### Detailné dokumenty

- **GTFS_DOKUMENTACIA.md** - Kompletný popis GTFS formátu a všetkých polí
- **JDF_FORMAT_DOKUMENTACIA.md** - Detailná špecifikácia JDF formátu
- **JDF_FORMAT_PRIKLADY.md** - Príklady JDF súborov s vysvetleniami
- **RYCHLY_START.md** - Rýchly úvod do projektu
- **JDF_RYCHLY_START.md** - Rýchly úvod do JDF

### Ďalšie zdroje

- **GTFS Referencia:** https://gtfs.org/documentation/schedule/reference/
- **GTFS Best Practices:** https://gtfs.org/best-practices/
- **JDF Špecifikácia:** Oficiálna dokumentácia slovenského formátu

---

## 🤝 Prispievanie

Projekt je otvorený pre príspevky. Pri prispievaní dodržujte:

1. Konzistentnú štruktúru kódu
2. XML dokumentačné komentáre pre verejné API
3. Unit testy pre novú funkcionalitu
4. Update dokumentácie pri zmenách

---

## 📝 Licencia

Tento projekt je voľne dostupný pre vzdelávacie a nekomerčné účely.

---

## 👨‍💻 Autor

Vytvorené ako školský projekt pre správu a konverziu dát verejnej dopravy.

---

## 🔗 Užitočné odkazy

- [GTFS Specification](https://gtfs.org/documentation/schedule/reference/)
- [Avalonia UI Documentation](https://docs.avaloniaui.net/)
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [CsvHelper Documentation](https://joshclose.github.io/CsvHelper/)

---

**Posledná aktualizácia:** December 2025

