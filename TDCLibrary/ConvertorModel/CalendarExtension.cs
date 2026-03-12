using System.Globalization;
using TDCLibrary.GtfsModel;
using TDCLibrary.GtfsModel.Enums;
using TDCLibrary.GtsfModel;
using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary.ConvertorModel;

public class CalendarExtension
{
    /// <summary>
    /// Reprezentuje časový úsek s pevnými kódmi
    /// </summary>
    public class TimeSegment
    {
        public List<PevnyKodOznacenie> PevneKody { get; set; } = new();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    
    private static readonly HashSet<DateTime> SlovakHolidays = new()
    {
        new DateTime(2026, 1, 1), // Deň vzniku SR
        new DateTime(2026, 1, 6), // Zjavenie Pána
        new DateTime(2026, 3, 29), // Veľký piatok
        new DateTime(2026, 4, 1), // Veľkonočný pondelok
        new DateTime(2026, 5, 1), // Sviatok práce
        new DateTime(2026, 7, 5), // Cyril a Metod
        new DateTime(2026, 8, 29), // SNP
        new DateTime(2026, 9, 1), // Deň Ústavy SR
        new DateTime(2026, 9, 15), // Sedembolestná Panna Mária
        new DateTime(2026, 11, 1), // Všetkých svätých
        new DateTime(2026, 11, 17), // Deň boja za slobodu a demokraciu
        new DateTime(2026, 12, 24), // Štedrý deň
        new DateTime(2026, 12, 25), // 1. sviatok vianočný
        new DateTime(2026, 12, 26) // 2. sviatok vianočný
    };

    /// <summary>
    /// Detekuje zmeny v období platnosti služby kvôli výnimkám (calendar_dates)
    /// Prechádza deň po dni a kontroluje calendar.txt (kedy má ísť) a calendar_dates.txt (kedy skutočne ide)
    /// Vytvára časové segmenty keď sa zmení režim prevádzky
    /// </summary>
    public static List<TimeSegment> GetTimeSegmentsForService(string serviceId, GtfsData gtsfData)
    {
        var segments = new List<TimeSegment>();
        
        var calendar = gtsfData.Calendars.FirstOrDefault(c => c.ServiceId == serviceId);
        var calendarDates = gtsfData.CalendarDates
            .Where(cd => cd.ServiceId == serviceId)
            .OrderBy(cd => cd.Date)
            .ToList();

        // Ak nie je calendar, použijeme iba calendar_dates
        if (calendar == null)
        {
            if (calendarDates.Count == 0)
            {
                return segments;
            }
            
            return ProcessCalendarDatesOnly(calendarDates);
        }

        // Určíme začiatok a koniec obdobia
        DateTime startDate = DateTime.ParseExact(calendar.StartDate, "yyyyMMdd", CultureInfo.InvariantCulture);
        DateTime endDate = DateTime.ParseExact(calendar.EndDate, "yyyyMMdd", CultureInfo.InvariantCulture);
        
        Console.WriteLine($"DEBUG GetTimeSegments: ServiceId {serviceId}: start={startDate:yyyy-MM-dd}, end={endDate:yyyy-MM-dd}");
        Console.WriteLine($"DEBUG: Calendar nastavenie - Po:{calendar.Monday}, Ut:{calendar.Tuesday}, St:{calendar.Wednesday}, Št:{calendar.Thursday}, Pi:{calendar.Friday}, So:{calendar.Saturday}, Ne:{calendar.Sunday}");
        
        // Vytvoríme mapu calendar_dates pre rýchle vyhľadávanie
        // V calendar_dates.txt je exception_type=1 znamená že služba IDE v tento deň
        var calendarDatesAddedMap = calendarDates
            .Where(cd => cd.ExceptionType == ExceptionType.ServiceAdded)
            .Select(cd => DateTime.ParseExact(cd.Date, "yyyyMMdd", CultureInfo.InvariantCulture))
            .ToHashSet();
        
        Console.WriteLine($"DEBUG: Našlo sa {calendarDatesAddedMap.Count} dátumov (ServiceAdded) v calendar_dates.txt pre service {serviceId}");
        if (calendarDatesAddedMap.Count > 0)
        {
            var sortedDates = calendarDatesAddedMap.OrderBy(d => d).ToList();
            Console.WriteLine($"DEBUG: Prvy datum v calendar_dates: {sortedDates.First():dd.MM.yyyy}, posledny: {sortedDates.Last():dd.MM.yyyy}");
            if (calendarDatesAddedMap.Count <= 20)
            {
                Console.WriteLine($"DEBUG: Vsetky datumy: {string.Join(", ", sortedDates.Select(d => d.ToString("dd.MM")))}");
            }
            else
            {
                Console.WriteLine($"DEBUG: Prvych 10 datumov: {string.Join(", ", sortedDates.Take(10).Select(d => d.ToString("dd.MM")))}");
            }
        }

        // Ak existujú calendar_dates, znamená to že IBA tie dni idú
        // (lebo všetky sú exception_type=1, nie sú tam ServiceRemoved)
        bool useOnlyCalendarDates = calendarDatesAddedMap.Count > 0;

        // Prechádzame deň po dni a zbierame dni kedy spoj ide
        var currentDate = startDate;
        TimeSegment? currentSegment = null;
        var daysInCurrentSegment = new List<DateTime>();
        
        int debugDayCount = 0;
        int debugRunCount = 0;
        int debugSkipCount = 0;
        
        while (currentDate <= endDate)
        {
            debugDayCount++;
            
            // KROK 1: Zisti, aký je to deň v týždni a či má v tento deň ísť podľa calendar.txt
            bool shouldRunByCalendar = currentDate.DayOfWeek switch
            {
                DayOfWeek.Monday => calendar.Monday,
                DayOfWeek.Tuesday => calendar.Tuesday,
                DayOfWeek.Wednesday => calendar.Wednesday,
                DayOfWeek.Thursday => calendar.Thursday,
                DayOfWeek.Friday => calendar.Friday,
                DayOfWeek.Saturday => calendar.Saturday,
                DayOfWeek.Sunday => calendar.Sunday,
                _ => false
            };
            
            // KROK 2: Určíme, či služba skutočne ide v tento deň
            bool actuallyRuns;
            
            if (useOnlyCalendarDates)
            {
                // Ak existujú calendar_dates, IBA tie dni ktoré sú v calendar_dates idú
                // Musí byť v calendar_dates AND musí byť deň kedy by mala ísť podľa calendar
                actuallyRuns = shouldRunByCalendar && calendarDatesAddedMap.Contains(currentDate);
                
                if (debugDayCount <= 30 && shouldRunByCalendar)
                {
                    bool inCalendarDates = calendarDatesAddedMap.Contains(currentDate);
                    Console.WriteLine($"DEBUG day {currentDate:dd.MM.yyyy} ({currentDate.DayOfWeek}): shouldRun={shouldRunByCalendar}, inCalDates={inCalendarDates}, actuallyRuns={actuallyRuns}");
                }
            }
            else
            {
                // Ak nemáme calendar_dates, služba ide podľa calendar.txt
                actuallyRuns = shouldRunByCalendar;
            }
            
            if (actuallyRuns)
            {
                debugRunCount++;
                // Služba ide v tento deň
                if (currentSegment == null)
                {
                    // Začni nový segment
                    currentSegment = new TimeSegment
                    {
                        StartDate = currentDate,
                        EndDate = currentDate
                    };
                    daysInCurrentSegment = new List<DateTime> { currentDate };
                    Console.WriteLine($"DEBUG: NEW SEGMENT started at {currentDate:dd.MM.yyyy} ({currentDate.DayOfWeek})");
                }
                else
                {
                    // Rozšír existujúci segment
                    currentSegment.EndDate = currentDate;
                    daysInCurrentSegment.Add(currentDate);
                }
            }
            else
            {
                debugSkipCount++;
                // Služba nejde v tento deň
                if (currentSegment != null)
                {
                    // Spoj nejde a máme otvorený segment
                    // Ukončíme segment IBA ak ide o deň kedy BY MALA ísť (podľa calendar), ale nejde
                    if (shouldRunByCalendar)
                    {
                        // Deň kedy by mala ísť (napr. pondelok) ale nejde (nie je v calendar_dates)
                        // Ulož aktuálny segment
                        currentSegment.PevneKody = DeterminePevneKody(daysInCurrentSegment);
                        segments.Add(currentSegment);
                        
                        Console.WriteLine($"DEBUG GetTimeSegments: SEGMENT ENDED on {currentDate:dd.MM.yyyy} (missing from calendar_dates): {currentSegment.StartDate:dd.MM.yyyy} - {currentSegment.EndDate:dd.MM.yyyy}, days={daysInCurrentSegment.Count}, pevneKody={string.Join(",", currentSegment.PevneKody)}");
                        
                        currentSegment = null;
                        daysInCurrentSegment = new List<DateTime>();
                    }
                    // Ak by ani nemala ísť podľa calendar.txt (napr. sobota pri Po-Pi rozvrhu), segment zostáva otvorený
                }
            }
            
            currentDate = currentDate.AddDays(1);
        }
        
        Console.WriteLine($"DEBUG: Processed {debugDayCount} days, {debugRunCount} run, {debugSkipCount} skipped");
        
        // Ulož posledný segment
        if (currentSegment != null)
        {
            currentSegment.PevneKody = DeterminePevneKody(daysInCurrentSegment);
            segments.Add(currentSegment);
            Console.WriteLine($"DEBUG GetTimeSegments: Final segment {currentSegment.StartDate:dd.MM.yyyy} - {currentSegment.EndDate:dd.MM.yyyy}, days={daysInCurrentSegment.Count}, pevneKody={string.Join(",", currentSegment.PevneKody)}");
        }
        
        Console.WriteLine($"DEBUG GetTimeSegments: Created {segments.Count} segments for serviceId {serviceId}");

        return segments;
    }

    /// <summary>
    /// Kontroluje, či je nový deň kompatibilný s existujúcimi dňami v segmente
    /// Rozhoduje, či pokračovať v segmente alebo začať nový
    /// </summary>
    private static bool AreCompatibleDays(List<DateTime> existingDays, DateTime newDay)
    {
        if (existingDays.Count == 0)
            return true;
            
        // Získame dni v týždni ktoré už máme
        var existingDaysOfWeek = existingDays.Select(d => d.DayOfWeek).ToHashSet();
        
        // Ak už máme tento deň v týždni, je kompatibilný
        if (existingDaysOfWeek.Contains(newDay.DayOfWeek))
        {
            return true;
        }
        
        // Skontroluj, či by pridanie nového dňa stále tvorilo rozumný vzor
        var allDaysOfWeek = new HashSet<DayOfWeek>(existingDaysOfWeek) { newDay.DayOfWeek };
        
        // Pracovné dni (Po-Pi) - ak všetko spĺňa tento vzor, pokračuj v segmente
        bool isPracovneDni = allDaysOfWeek.IsSubsetOf(new HashSet<DayOfWeek>
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, 
            DayOfWeek.Thursday, DayOfWeek.Friday
        });
        
        if (isPracovneDni)
        {
            return true;
        }
        
        // Víkend (So-Ne)
        bool isVikend = allDaysOfWeek.IsSubsetOf(new HashSet<DayOfWeek>
        {
            DayOfWeek.Saturday, DayOfWeek.Sunday
        });
        
        if (isVikend)
        {
            return true;
        }
        
        // Ak nie je žiadny jasný vzor, začni nový segment
        return false;
    }

    /// <summary>
    /// Určí pevné kódy na základe zoznamu dní v segmente
    /// </summary>
    private static List<PevnyKodOznacenie> DeterminePevneKody(List<DateTime> days)
    {
        var pevneKody = new List<PevnyKodOznacenie>();
        
        if (days.Count == 0)
        {
            return pevneKody;
        }
        
        // Analyzuj, ktoré dni v týždni sú v segmente
        var daysOfWeek = days
            .GroupBy(d => d.DayOfWeek)
            .Select(g => g.Key)
            .ToHashSet();
        
        bool monday = daysOfWeek.Contains(DayOfWeek.Monday);
        bool tuesday = daysOfWeek.Contains(DayOfWeek.Tuesday);
        bool wednesday = daysOfWeek.Contains(DayOfWeek.Wednesday);
        bool thursday = daysOfWeek.Contains(DayOfWeek.Thursday);
        bool friday = daysOfWeek.Contains(DayOfWeek.Friday);
        bool saturday = daysOfWeek.Contains(DayOfWeek.Saturday);
        bool sunday = daysOfWeek.Contains(DayOfWeek.Sunday);
        
        // Kontrola, či chodi Po-Pi (pracovné dni)
        bool isPracovneDni = monday && tuesday && wednesday && thursday && friday;
        if (isPracovneDni)
        {
            pevneKody.Add(PevnyKodOznacenie.PracovneDni);
        }
        else
        {
            if (monday) pevneKody.Add(PevnyKodOznacenie.Pondelok);
            if (tuesday) pevneKody.Add(PevnyKodOznacenie.Utorok);
            if (wednesday) pevneKody.Add(PevnyKodOznacenie.Streda);
            if (thursday) pevneKody.Add(PevnyKodOznacenie.Stvrtok);
            if (friday) pevneKody.Add(PevnyKodOznacenie.Piatok);
        }
        
        if (saturday)
        {
            pevneKody.Add(PevnyKodOznacenie.Sobota);
        }
        
        if (sunday)
        {
            // Kontrola, či ide aj cez sviatky
            bool ideVoSviatky = days.Any(d => SlovakHolidays.Contains(d));
            pevneKody.Add(ideVoSviatky ? 
                PevnyKodOznacenie.NedeleAveSviatky : PevnyKodOznacenie.Nedela);
        }
        
        return pevneKody;
    }

    /// <summary>
    /// Spracuje iba calendar_dates bez calendar.txt
    /// Prechádza deň po dni a vytvára segmenty s pevnými kódmi
    /// </summary>
    private static List<TimeSegment> ProcessCalendarDatesOnly(List<CalendarDate> calendarDates)
    {
        var segments = new List<TimeSegment>();
        
        // Filtruj iba dni kedy služba ide (ServiceAdded)
        var activeDates = calendarDates
            .Where(cd => cd.ExceptionType == ExceptionType.ServiceAdded)
            .Select(cd => DateTime.ParseExact(cd.Date, "yyyyMMdd", CultureInfo.InvariantCulture))
            .OrderBy(d => d)
            .ToList();

        if (activeDates.Count == 0)
        {
            return segments;
        }

        // Prechádzame všetky aktívne dni a vytvárame kontinuálne segmenty
        TimeSegment? currentSegment = null;
        var daysInCurrentSegment = new List<DateTime>();
        
        foreach (var date in activeDates)
        {
            if (currentSegment == null)
            {
                // Začni nový segment
                currentSegment = new TimeSegment
                {
                    StartDate = date,
                    EndDate = date
                };
                daysInCurrentSegment = new List<DateTime> { date };
            }
            else
            {
                // Vypočítaj počet dní medzi aktuálnym a posledným dňom
                int daysBetween = (int)(date - currentSegment.EndDate).TotalDays;
                
                // Ak je medzera väčšia ako 1 deň, vytvor nový segment
                if (daysBetween > 1)
                {
                    // Ulož aktuálny segment s jeho pevnými kódmi
                    currentSegment.PevneKody = DeterminePevneKody(daysInCurrentSegment);
                    segments.Add(currentSegment);
                    
                    Console.WriteLine($"DEBUG ProcessCalendarDatesOnly: Segment {currentSegment.StartDate:dd.MM.yyyy} - {currentSegment.EndDate:dd.MM.yyyy}, days={daysInCurrentSegment.Count}, pevneKody={string.Join(",", currentSegment.PevneKody)}");
                    
                    // Začni nový segment
                    currentSegment = new TimeSegment
                    {
                        StartDate = date,
                        EndDate = date
                    };
                    daysInCurrentSegment = new List<DateTime> { date };
                }
                else
                {
                    // Pokračuj v aktuálnom segmente
                    currentSegment.EndDate = date;
                    daysInCurrentSegment.Add(date);
                }
            }
        }

        // Ulož posledný segment
        if (currentSegment != null)
        {
            currentSegment.PevneKody = DeterminePevneKody(daysInCurrentSegment);
            segments.Add(currentSegment);
            Console.WriteLine($"DEBUG ProcessCalendarDatesOnly: Final segment {currentSegment.StartDate:dd.MM.yyyy} - {currentSegment.EndDate:dd.MM.yyyy}, days={daysInCurrentSegment.Count}, pevneKody={string.Join(",", currentSegment.PevneKody)}");
        }

        return segments;
    }

    /// <summary>
    /// Vráti pevné kódy pre konkrétny deň
    /// </summary>
    private static List<PevnyKodOznacenie> GetPevneKodyForDay(
        DateTime date, 
        TDCLibrary.GtfsModel.Calendar calendar, 
        Dictionary<DateTime, ExceptionType> exceptions)
    {
        var pevneKody = new List<PevnyKodOznacenie>();
        
        // Skontroluj výnimky
        if (exceptions.TryGetValue(date, out var exceptionType))
        {
            if (exceptionType == ExceptionType.ServiceRemoved)
            {
                return pevneKody; // Služba nejde v tento deň
            }
            // ServiceAdded - služba ide aj keď normálne by nešla
        }
        else
        {
            // Nie je výnimka, skontroluj calendar
            bool isActive = date.DayOfWeek switch
            {
                DayOfWeek.Monday => calendar.Monday,
                DayOfWeek.Tuesday => calendar.Tuesday,
                DayOfWeek.Wednesday => calendar.Wednesday,
                DayOfWeek.Thursday => calendar.Thursday,
                DayOfWeek.Friday => calendar.Friday,
                DayOfWeek.Saturday => calendar.Saturday,
                DayOfWeek.Sunday => calendar.Sunday,
                _ => false
            };
            
            if (!isActive)
            {
                return pevneKody; // Služba nejde v tento deň
            }
        }

        // Služba ide v tento deň - urči pevné kódy
        switch (date.DayOfWeek)
        {
            case DayOfWeek.Monday:
                pevneKody.Add(PevnyKodOznacenie.Pondelok);
                break;
            case DayOfWeek.Tuesday:
                pevneKody.Add(PevnyKodOznacenie.Utorok);
                break;
            case DayOfWeek.Wednesday:
                pevneKody.Add(PevnyKodOznacenie.Streda);
                break;
            case DayOfWeek.Thursday:
                pevneKody.Add(PevnyKodOznacenie.Stvrtok);
                break;
            case DayOfWeek.Friday:
                pevneKody.Add(PevnyKodOznacenie.Piatok);
                break;
            case DayOfWeek.Saturday:
                pevneKody.Add(PevnyKodOznacenie.Sobota);
                break;
            case DayOfWeek.Sunday:
                bool isSviatokOrNedela = SlovakHolidays.Contains(date);
                pevneKody.Add(isSviatokOrNedela ? 
                    PevnyKodOznacenie.NedeleAveSviatky : PevnyKodOznacenie.Nedela);
                break;
        }

        return pevneKody;
    }

    /// <summary>
    /// Vráti pevné kódy pre konkrétny dátum z calendar_dates
    /// </summary>
    private static List<PevnyKodOznacenie> GetPevneKodyForDate(DateTime date)
    {
        var pevneKody = new List<PevnyKodOznacenie>();

        switch (date.DayOfWeek)
        {
            case DayOfWeek.Monday:
                pevneKody.Add(PevnyKodOznacenie.Pondelok);
                break;
            case DayOfWeek.Tuesday:
                pevneKody.Add(PevnyKodOznacenie.Utorok);
                break;
            case DayOfWeek.Wednesday:
                pevneKody.Add(PevnyKodOznacenie.Streda);
                break;
            case DayOfWeek.Thursday:
                pevneKody.Add(PevnyKodOznacenie.Stvrtok);
                break;
            case DayOfWeek.Friday:
                pevneKody.Add(PevnyKodOznacenie.Piatok);
                break;
            case DayOfWeek.Saturday:
                pevneKody.Add(PevnyKodOznacenie.Sobota);
                break;
            case DayOfWeek.Sunday:
                bool isSviatok = SlovakHolidays.Contains(date);
                pevneKody.Add(isSviatok ? 
                    PevnyKodOznacenie.NedeleAveSviatky : PevnyKodOznacenie.Nedela);
                break;
        }

        return pevneKody;
    }

    /// <summary>
    /// Porovná dva zoznamy pevných kódov
    /// </summary>
    private static bool ArePevneKodyEqual(List<PevnyKodOznacenie> list1, List<PevnyKodOznacenie> list2)
    {
        if (list1.Count != list2.Count)
            return false;
            
        var set1 = new HashSet<PevnyKodOznacenie>(list1);
        var set2 = new HashSet<PevnyKodOznacenie>(list2);
        
        return set1.SetEquals(set2);
    }

    public static List<PevnyKodOznacenie> MapServiceIdToPevnyKod(string serviceId, GtfsData gtsfData)
    {
        var pevneKody = new List<PevnyKodOznacenie>();

        var calendar = gtsfData.Calendars.FirstOrDefault(c => c.ServiceId == serviceId);
        var calendarDates = gtsfData.CalendarDates
            .Where(cd => cd.ServiceId == serviceId)
            .ToList();

        // Check if calendar exists and has at least one active day
        bool hasCalendarWithActiveDays = calendar != null && 
            (calendar.Monday || calendar.Tuesday || calendar.Wednesday || 
             calendar.Thursday || calendar.Friday || calendar.Saturday || calendar.Sunday);

        // Ak nie je calendar.txt alebo nemá žiadne aktívne dni, použijeme iba calendar_dates.txt
        if (!hasCalendarWithActiveDays)
        {
            if (calendarDates.Count == 0)
            {
                Console.Error.WriteLine($"No calendar_dates found for serviceId: {serviceId}");
                return new List<PevnyKodOznacenie>();
            }

            // Analyzujeme calendar_dates - dni kedy služba ide (ServiceAdded)
            var activeDates = calendarDates
                .Where(cd => cd.ExceptionType == ExceptionType.ServiceAdded)
                .Select(cd => DateTime.ParseExact(cd.Date, "yyyyMMdd", CultureInfo.InvariantCulture))
                .ToList();

            if (activeDates.Count == 0)
            {
                Console.Error.WriteLine($"No active dates found for serviceId: {serviceId}");
                return new List<PevnyKodOznacenie>();
            }

            // Analyzujeme, v ktoré dni týždňa služba chodí
            var daysOfWeek = activeDates
                .GroupBy(d => d.DayOfWeek)
                .ToDictionary(g => g.Key, g => g.Count());

            Console.WriteLine($"Service {serviceId} operates on: {string.Join(", ", daysOfWeek.Keys)}");

            bool monday = daysOfWeek.ContainsKey(DayOfWeek.Monday);
            bool tuesday = daysOfWeek.ContainsKey(DayOfWeek.Tuesday);
            bool wednesday = daysOfWeek.ContainsKey(DayOfWeek.Wednesday);
            bool thursday = daysOfWeek.ContainsKey(DayOfWeek.Thursday);
            bool friday = daysOfWeek.ContainsKey(DayOfWeek.Friday);
            bool saturday = daysOfWeek.ContainsKey(DayOfWeek.Saturday);
            bool sunday = daysOfWeek.ContainsKey(DayOfWeek.Sunday);

            // Kontrola, či chodi Po-Pi (pracovné dni)
            bool isPracovneDni = monday && tuesday && wednesday && thursday && friday;
            if (isPracovneDni)
            {
                pevneKody.Add(PevnyKodOznacenie.PracovneDni);
                Console.WriteLine($"Service {serviceId}: Pracovné dni");
            }
            else
            {
                if (monday) pevneKody.Add(PevnyKodOznacenie.Pondelok);
                if (tuesday) pevneKody.Add(PevnyKodOznacenie.Utorok);
                if (wednesday) pevneKody.Add(PevnyKodOznacenie.Streda);
                if (thursday) pevneKody.Add(PevnyKodOznacenie.Stvrtok);
                if (friday) pevneKody.Add(PevnyKodOznacenie.Piatok);
            }

            if (saturday)
            {
                pevneKody.Add(PevnyKodOznacenie.Sobota);
                Console.WriteLine($"Service {serviceId}: Sobota");
            }

            if (sunday)
            {
                // Kontrola, či ide aj cez sviatky
                bool ideVoSviatky = activeDates.Any(d => SlovakHolidays.Contains(d));
                
                if (ideVoSviatky)
                {
                    pevneKody.Add(PevnyKodOznacenie.NedeleAveSviatky);
                    Console.WriteLine($"Service {serviceId}: Nedeľa a sviatky");
                }
                else
                {
                    pevneKody.Add(PevnyKodOznacenie.Nedela);
                    Console.WriteLine($"Service {serviceId}: Nedeľa");
                }
            }

            return pevneKody.Distinct().ToList();
        }

        // Ak existuje calendar.txt, použijeme pôvodný prístup
        Console.WriteLine($"DEBUG Calendar: ServiceId={serviceId}, Mon={calendar.Monday}, Tue={calendar.Tuesday}, Wed={calendar.Wednesday}, Thu={calendar.Thursday}, Fri={calendar.Friday}, Sat={calendar.Saturday}, Sun={calendar.Sunday}");

        // Kontrola, či chodi Po-Pi (pracovné dni)
        bool isPracovneDni2 = calendar.Monday && calendar.Tuesday && calendar.Wednesday &&
                             calendar.Thursday && calendar.Friday;
        if (isPracovneDni2)
        {
            pevneKody.Add(PevnyKodOznacenie.PracovneDni);
            Console.WriteLine($"DEBUG: Priradilo pracovne dni pre serviceId {serviceId}");
        }
        else
        {
            if (calendar.Monday) pevneKody.Add(PevnyKodOznacenie.Pondelok);
            if (calendar.Tuesday) pevneKody.Add(PevnyKodOznacenie.Utorok);
            if (calendar.Wednesday) pevneKody.Add(PevnyKodOznacenie.Streda);
            if (calendar.Thursday) pevneKody.Add(PevnyKodOznacenie.Stvrtok);
            if (calendar.Friday) pevneKody.Add(PevnyKodOznacenie.Piatok);
        }

        if (calendar.Saturday)
        {
            pevneKody.Add(PevnyKodOznacenie.Sobota);
            Console.WriteLine($"DEBUG: Priradilo sobotu pre serviceId {serviceId}");
        }

        if (calendar.Sunday)
        {
            bool nechodiVoSviatky = calendarDates.Any(cd =>
                cd.ExceptionType == ExceptionType.ServiceRemoved &&
                SlovakHolidays.Contains(DateTime.ParseExact(cd.Date, "yyyyMMdd", CultureInfo.InvariantCulture)));

            if (nechodiVoSviatky)
            {
                pevneKody.Add(PevnyKodOznacenie.Nedela);
                Console.WriteLine($"DEBUG: Priradilo nedelu pre serviceId {serviceId}");
            }
            else
            {
                pevneKody.Add(PevnyKodOznacenie.NedeleAveSviatky);
                Console.WriteLine($"DEBUG: Priradilo nedele a sviatky pre serviceId {serviceId}");
            }
        }

        Console.WriteLine($"DEBUG: Celkovo pevnych kodov pre serviceId {serviceId}: {pevneKody.Count}");
        return pevneKody.Distinct().ToList();
    }

    public static TypCasKod ToTypCasKod(ExceptionType exception)
    {
        if (exception == ExceptionType.ServiceAdded)
        {
            return TypCasKod.JedeJen;
        }
        
        if (exception == ExceptionType.ServiceRemoved)
        {
            return TypCasKod.Nejede;
        }

        throw new ArgumentException("Not valid state");
    }
}