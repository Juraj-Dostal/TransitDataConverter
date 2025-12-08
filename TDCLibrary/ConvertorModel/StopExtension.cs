namespace TDCLibrary.ConvertorModel;

public class StopExtension
{
    /// <summary>
    /// Rozdeluje id zastavky z gtfs na jdf z BA dát. Nemusi byť všeobečné riešenie.
    /// </summary>
    /// <param name="stopId"></param>
    /// <returns>(CisloZastavky, int KodOznaciku)</returns>  
    public static (int CisloZastavky, int KodOznaciku) SplitStopId(string stopId)
    {
        if (string.IsNullOrWhiteSpace(stopId))
        {
            return (0, 0);
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
}