using TDCLibrary.GtsfModel;
using TDCLibrary.GtsfModel.Enums;
using TDCLibrary.JdfModel;
using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary.ConvertorModel;

public class CalendarExtension
{
    private static readonly HashSet<DateTime> SlovakHolidays = new()
    {
        new DateTime(2024, 1, 1), // Deň vzniku SR
        new DateTime(2024, 1, 6), // Zjavenie Pána
        new DateTime(2024, 3, 29), // Veľký piatok
        new DateTime(2024, 4, 1), // Veľkonočný pondelok
        new DateTime(2024, 5, 1), // Sviatok práce
        new DateTime(2024, 5, 8), // Deň víťazstva nad fašizmom
        new DateTime(2024, 7, 5), // Cyril a Metod
        new DateTime(2024, 8, 29), // SNP
        new DateTime(2024, 9, 1), // Deň Ústavy SR
        new DateTime(2024, 9, 15), // Sedembolestná Panna Mária
        new DateTime(2024, 11, 1), // Všetkých svätých
        new DateTime(2024, 11, 17), // Deň boja za slobodu a demokraciu
        new DateTime(2024, 12, 24), // Štedrý deň
        new DateTime(2024, 12, 25), // 1. sviatok vianočný
        new DateTime(2024, 12, 26) // 2. sviatok vianočný
    };

    public static List<PevnyKodOznacenie> MapServiceIdToPevnyKod(string serviceId, GtfsData gtsfData)
    {
        var pevneKody = new List<PevnyKodOznacenie>();

        var calendar = gtsfData.Calendars.FirstOrDefault(c => c.ServiceId == serviceId);
        if (calendar == null)
        {
            Console.Error.WriteLine($"Calendar not found for serviceId: {serviceId}");
            return new List<PevnyKodOznacenie>();
        }

        var calendarDates = gtsfData.CalendarDates
            .Where(cd => cd.ServiceId == serviceId)
            .ToList();

        // Kontrola, či chodi Po-Pi (pracovné dni)
        bool isPracovneDni = calendar.Monday && calendar.Tuesday && calendar.Wednesday &&
                             calendar.Thursday && calendar.Friday;
        if (isPracovneDni)
        {
            pevneKody.Add(PevnyKodOznacenie.PracovneDni);
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
        }

        if (calendar.Sunday)
        {
            bool nechodiVoSviatky = calendarDates.Any(cd =>
                cd.ExceptionType == ExceptionType.ServiceRemoved && // pridaný deň
                SlovakHolidays.Contains(DateTime.ParseExact(cd.Date, "yyyyMMdd", null)));

            // Ak chodi v nedeľu alebo vo sviatky
            if (calendar.Sunday || nechodiVoSviatky)
            {
                pevneKody.Add(PevnyKodOznacenie.Nedela);
            }
            else
            {
                pevneKody.Add(PevnyKodOznacenie.NedeleAveSviatky);
            }
        }

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