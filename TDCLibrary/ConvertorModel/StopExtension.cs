namespace TDCLibrary.ConvertorModel;

public class StopExtension
{
    /// <summary>
    /// Rozdeluje id zastavky z gtfs na jdf z BA dát. Nemusi byť všeobečné riešenie.
    /// Podporuje formáty:
    /// - "U488Z1" -> CisloZastavky: 488, KodOznaciku: 1
    /// - "1234567" -> CisloZastavky: 123, KodOznaciku: 45
    /// </summary>
    /// <param name="stopId"></param>
    /// <returns>(CisloZastavky, int KodOznaciku)</returns>  
    public static (int CisloZastavky, int KodOznaciku) SplitStopId(string stopId)
    {
        if (string.IsNullOrWhiteSpace(stopId))
        {
            return (0, 0);
        }

        // Kontrola formátu ako "U488Z1" - písmená a čísla striedavo
        // Hľadáme pattern: písmená, potom čísla, potom písmená, potom čísla
        var match = System.Text.RegularExpressions.Regex.Match(stopId, @"[U]+(\d+)[Z]+(\d+)");
        if (match.Success)
        {
            int cisloZastavky = int.TryParse(match.Groups[1].Value, out var cz) ? cz : 0;
            int kodOznaciku = int.TryParse(match.Groups[2].Value, out var ko) ? ko : 0;
            return (cisloZastavky, kodOznaciku);
        }

        var digits = new string(stopId.Where(char.IsDigit).ToArray());
        if (digits.Length == 0)
        {
            return (0, 0);
        }

        if (digits.Length <= 2)
        {
            if (int.TryParse(digits, out int value))
                return (0, value);
            return (0, 0);
        }

        var prefixStr = digits.Substring(0, digits.Length - 2);
        var suffixStr = digits.Substring(digits.Length - 2);
        
        int prefix = int.TryParse(prefixStr, out var p) ? p : 0;
        int suffix = int.TryParse(suffixStr, out var s) ? s : 0;

        return (prefix, suffix);
    }
    
    /// <summary>
    /// Rozdeluje názov zastávky na časť obec a po blízke miesto.
    /// </summary>
    /// <param name="stopName">Celý názov zastávky (napr. "Bratislava, Patrónka")</param>
    /// <returns>(string Obec, string BlizkeMiesto)) - tuple s časťou pred a po čiarke</returns>
    public static (string Obec, string BlizkeMiesto) SplitStopName(string stopName)
    {
        if (string.IsNullOrWhiteSpace(stopName))
        {
            return (string.Empty, string.Empty);
        }

        var parts = stopName.Split(',', 2);
    
        if (parts.Length == 1)
        {
            return (parts[0].Trim(), string.Empty);
        }

        return (parts[0].Trim(), parts[1].Trim());
    }
}