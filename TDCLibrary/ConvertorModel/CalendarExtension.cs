using TDCLibrary.GtfsModel.Enums;
using TDCLibrary.GtsfModel;
using TDCLibrary.JdfModel;
using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary.ConvertorModel;

public class CalendarExtension
{
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
            Console.WriteLine($"Calendar not found or has no active days for serviceId: {serviceId}, analyzing calendar_dates...");
            
            if (calendarDates.Count == 0)
            {
                Console.Error.WriteLine($"No calendar_dates found for serviceId: {serviceId}");
                return new List<PevnyKodOznacenie>();
            }

            // Analyzujeme calendar_dates - dni kedy služba ide (ServiceAdded)
            var activeDates = calendarDates
                .Where(cd => cd.ExceptionType == ExceptionType.ServiceAdded)
                .Select(cd => DateTime.ParseExact(cd.Date, "yyyyMMdd", null))
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
                SlovakHolidays.Contains(DateTime.ParseExact(cd.Date, "yyyyMMdd", null)));

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
