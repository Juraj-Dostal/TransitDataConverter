using TDCLibrary;
using TDCLibrary.GtsfModel.Enums;

namespace TDCLibrary.Examples;

/// <summary>
/// Príklad testovania CemvSupport enum
/// </summary>
public class CemvSupportExample
{
    /// <summary>
    /// Testuje všetky tri hodnoty CemvSupport enum
    /// </summary>
    public static void TestCemvSupportValues()
    {
        Console.WriteLine("=== TEST CemvSupport Enum ===\n");
        
        // Test hodnoty 0
        TestCemvValue(CemvSupport.NoInformation, 0, "NoInformation");
        
        // Test hodnoty 1
        TestCemvValue(CemvSupport.Supported, 1, "Supported");
        
        // Test hodnoty 2
        TestCemvValue(CemvSupport.NotSupported, 2, "NotSupported");
        
        Console.WriteLine("\n✅ Všetky hodnoty sú správne!");
    }
    
    private static void TestCemvValue(CemvSupport cemv, int expectedValue, string expectedName)
    {
        int actualValue = (int)cemv;
        string actualName = cemv.ToString();
        
        Console.WriteLine($"Enum: {actualName}");
        Console.WriteLine($"  Očakávaná hodnota: {expectedValue}");
        Console.WriteLine($"  Skutočná hodnota: {actualValue}");
        Console.WriteLine($"  Status: {(actualValue == expectedValue ? "✅ OK" : "❌ CHYBA")}");
        Console.WriteLine();
    }
    
    /// <summary>
    /// Simulácia načítania GTFS agency.txt s rôznymi cemv_support hodnotami
    /// </summary>
    public static void SimulateGtfsAgencyParsing()
    {
        Console.WriteLine("=== Simulácia GTFS agency.txt parsovania ===\n");
        
        // Simulované dáta z CSV
        var testCases = new[]
        {
            ("Dopravný podnik A", "0", "žiadna informácia"),
            ("Dopravný podnik B", "1", "podporované"),
            ("Dopravný podnik C", "2", "nepodporované"),
            ("Dopravný podnik D", "", "prázdne pole")
        };
        
        foreach (var (name, value, expected) in testCases)
        {
            CemvSupport? cemv = ParseCemvSupport(value);
            
            Console.WriteLine($"Agentúra: {name}");
            Console.WriteLine($"  CSV hodnota: '{value}'");
            Console.WriteLine($"  Očakávaný výsledok: {expected}");
            Console.WriteLine($"  Parsovaný enum: {cemv?.ToString() ?? "null"}");
            Console.WriteLine($"  Číselná hodnota: {(cemv.HasValue ? ((int)cemv.Value).ToString() : "null")}");
            
            // Overenie správnosti
            string result = cemv switch
            {
                CemvSupport.NoInformation => "žiadna informácia",
                CemvSupport.Supported => "podporované",
                CemvSupport.NotSupported => "nepodporované",
                null => "prázdne pole",
                _ => "neznáme"
            };
            
            Console.WriteLine($"  Status: {(result == expected ? "✅ SPRÁVNE" : "❌ CHYBNÉ")}");
            Console.WriteLine();
        }
    }
    
    private static CemvSupport? ParseCemvSupport(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        
        if (int.TryParse(value, out var intValue))
        {
            if (Enum.IsDefined(typeof(CemvSupport), intValue))
            {
                return (CemvSupport)intValue;
            }
        }
        
        return null;
    }
    
    /// <summary>
    /// Príklad použitia v reálnom kóde
    /// </summary>
    public static void RealWorldExample(string gtfsPath)
    {
        Console.WriteLine("=== Reálny príklad použitia ===\n");
        
        var loader = new GtfsLoader(gtfsPath);
        var gtfsData = loader.LoadAll();
        
        Console.WriteLine($"Načítaných {gtfsData.Agencies.Count} agentúr:\n");
        
        foreach (var agency in gtfsData.Agencies)
        {
            Console.WriteLine($"📍 {agency.AgencyName}");
            
            switch (agency.CemvSupport)
            {
                case CemvSupport.Supported:
                    Console.WriteLine("   💳 cEMV: ✅ Podporované (hodnota: 1)");
                    Console.WriteLine("   → Cestujúci môžu použiť bezkontaktné karty");
                    break;
                    
                case CemvSupport.NotSupported:
                    Console.WriteLine("   💳 cEMV: ❌ Nepodporované (hodnota: 2)");
                    Console.WriteLine("   → Bezkontaktné karty nie sú akceptované");
                    break;
                    
                case CemvSupport.NoInformation:
                    Console.WriteLine("   💳 cEMV: ℹ️  Bez informácie (hodnota: 0)");
                    break;
                    
                default:
                    Console.WriteLine("   💳 cEMV: ℹ️  Údaj chýba (null)");
                    break;
            }
            
            Console.WriteLine();
        }
        
        // Štatistiky
        int supported = gtfsData.Agencies.Count(a => a.CemvSupport == CemvSupport.Supported);
        int notSupported = gtfsData.Agencies.Count(a => a.CemvSupport == CemvSupport.NotSupported);
        int noInfo = gtfsData.Agencies.Count(a => a.CemvSupport == CemvSupport.NoInformation || !a.CemvSupport.HasValue);
        
        Console.WriteLine("📊 Štatistiky:");
        Console.WriteLine($"   ✅ S cEMV podporou: {supported}");
        Console.WriteLine($"   ❌ Bez cEMV podpory: {notSupported}");
        Console.WriteLine($"   ℹ️  Bez informácie: {noInfo}");
    }
}
