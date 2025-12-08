using System.ComponentModel;
using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary.JdfModel;

/// <summary>
/// Pevný kód - zoznam pevných kódov použitých na linke (Pevnykod.txt - POVINNÝ súbor)
/// </summary>
public class Pevnykod
{
    // Číslo pevného kódu - povinné (max. pětimístné) číslo DbString (5)
    // Označení pevného kódu - povinný text, max. 1 znak *a) DbString (1)
    // Rezerva - nepovinný text DbString (254)
    
    /// <summary>
    /// Číslo pevného kódu (POVINNÉ)
    /// </summary>
    public string Cislo { get; set; }
    
    /// <summary>
    /// Označenie pevného kódu (POVINNÉ)
    /// </summary>
    public string Oznacenie{ get; set; }
    
    /// <summary>
    /// Rezerva (VOLITEĽNÉ)
    /// </summary>
    public string? Rezerva { get; set; }
}

/// <summary>
/// Extension metódy na prácu s PevnyKod enumom
/// </summary>
public static class PevnyKodExtensions
{
    /// <summary>
    /// Vracia symbol (znak) pre danú enum hodnotu
    /// </summary>
    public static string ZiskajZnak(this PevnyKodOznacenie kod)
    {
        var type = kod.GetType();
        var memInfo = type.GetMember(kod.ToString());
        
        if (memInfo.Length > 0)
        {
            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs.Length > 0)
                return ((DescriptionAttribute)attrs[0]).Description;
        }
        
        return kod.ToString();
    }
    
    /// <summary>
    /// Konvertuje symbol (znak) na enum hodnotu
    /// </summary>
    public static PevnyKodOznacenie? ZoZnaku(string znak)
    {
        if (string.IsNullOrEmpty(znak))
            return null;
        
        var type = typeof(PevnyKodOznacenie);
        var fields = type.GetFields();
        
        foreach (var field in fields)
        {
            var attrs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs.Length > 0)
            {
                var description = ((DescriptionAttribute)attrs[0]).Description;
                if (description == znak)
                {
                    if (Enum.TryParse(field.Name, out PevnyKodOznacenie result))
                        return result;
                }
            }
        }
        
        return null;
    }
    
    /// <summary>
    /// Vracia názov enum hodnoty
    /// </summary>
    public static string ZiskajNazov(this PevnyKodOznacenie kod)
    {
        return kod.ToString();
    }
    
    /// <summary>
    /// Vracia všetky dostupné enum hodnoty s ich znakmi
    /// </summary>
    public static Dictionary<PevnyKodOznacenie, string> ZiskajVsetky()
    {
        return Enum.GetValues(typeof(PevnyKodOznacenie))
            .Cast<PevnyKodOznacenie>()
            .ToDictionary(k => k, v => v.ZiskajZnak());
    }
    
    /// <summary>
    /// Zakladný formát čísla pevného kódu (päťmiestne číslo s úvodnými nulami)
    /// </summary>
    /// <returns></returns>
    public static string DajCislo(PevnyKodOznacenie pevnyKodOznacenie)
    {
        int value = Convert.ToInt32(pevnyKodOznacenie);
        return value.ToString("D5");
    }
    
    /// <summary>
    /// Konverzia z čísla (string alebo int) na enum PevnyKodOznacenie
    /// </summary>
    public static PevnyKodOznacenie? ZoCisla(string? cislo)
    {
        if (string.IsNullOrWhiteSpace(cislo))
            return null;
            
        if (int.TryParse(cislo, out var intValue))
        {
            if (Enum.IsDefined(typeof(PevnyKodOznacenie), intValue))
            {
                return (PevnyKodOznacenie)intValue;
            }
        }
        
        return null;
    }
    
    /// <summary>
    /// Konverzia z čísla (int) na enum PevnyKodOznacenie
    /// </summary>
    public static PevnyKodOznacenie? ZoCisla(int? cislo)
    {
        if (!cislo.HasValue)
            return null;
            
        if (Enum.IsDefined(typeof(PevnyKodOznacenie), cislo.Value))
        {
            return (PevnyKodOznacenie)cislo.Value;
        }
        
        return null;
    }
}