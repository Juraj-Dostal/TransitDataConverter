using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TDCLibrary.JdfModel;
using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary;

/// <summary>
/// Načítavač JDF (Jednotný Dátový Formát) dát zo súborov.
/// Podporuje načítanie všetkých súborov JDF formátu pre údaje o doprave.
/// </summary>
public class JdfLoader
{
    private readonly string _jdfDirectoryPath;
    
    /// <summary>
    /// Inicializuje nový načítavač JDF dát.
    /// </summary>
    /// <param name="jdfDirectoryPath">Cesta k adresáru obsahujúcemu JDF súbory</param>
    public JdfLoader(string jdfDirectoryPath)
    {
        _jdfDirectoryPath = jdfDirectoryPath ?? throw new ArgumentNullException(nameof(jdfDirectoryPath));
        
        if (!Directory.Exists(jdfDirectoryPath))
        {
            throw new DirectoryNotFoundException($"JDF adresár nebol najdený: {jdfDirectoryPath}");
        }
    }
    
    /// <summary>
    /// Načíta všetky dostupné JDF dáta z adresára.
    /// Načíta len súbory, ktoré existujú - voliteľné súbory nemusia byť prítomné.
    /// </summary>
    /// <returns>Objekt obsahujúci všetky načítané JDF dáta</returns>
    public JdfData LoadAll()
    {
        var data = new JdfData();
        
        // Povinné súbory
        data.VerzeJDF = LoadVerzeJDF();
        data.Dopravci = LoadDopravci();
        data.Zastavky = LoadZastavky();
        data.Linky = LoadLinky();
        data.Zaslinky = LoadZaslinky();
        data.Spoje = LoadSpoje();
        data.Zasspoje = LoadZasspoje();
        data.Caskody = LoadCaskody();
        data.PevnyKod = LoadPevnyKod();
        
        // Voliteľné súbory
        data.Oznacniky = LoadOznacniky();
        
        return data;
    }
    
    /// <summary>
    /// Načíta verziu JDF zo súboru VerzeJDF.txt
    /// </summary>
    public VerzeJDF? LoadVerzeJDF()
    {
        var items = LoadCsvFile("VerzeJDF.txt", ParseVerzeJDF);
        return items.FirstOrDefault();
    }
    
    /// <summary>
    /// Načíta dopravné spoločnosti zo súboru Dopravci.txt
    /// </summary>
    public List<Dopravci> LoadDopravci()
    {
        return LoadCsvFile("Dopravci.txt", ParseDopravci);
    }
    
    /// <summary>
    /// Načíta zastávky zo súboru Zastavky.txt
    /// </summary>
    public List<Zastavky> LoadZastavky()
    {
        return LoadCsvFile("Zastavky.txt", ParseZastavky);
    }
    
    /// <summary>
    /// Načíta linky zo súboru Linky.txt
    /// </summary>
    public List<Linky> LoadLinky()
    {
        return LoadCsvFile("Linky.txt", ParseLinky);
    }
    
    /// <summary>
    /// Načíta zastávky liniek zo súboru Zaslinky.txt
    /// </summary>
    public List<Zaslinky> LoadZaslinky()
    {
        return LoadCsvFile("Zaslinky.txt", ParseZaslinky);
    }
    
    /// <summary>
    /// Načíta spoje zo súboru Spoje.txt
    /// </summary>
    public List<Spoje> LoadSpoje()
    {
        return LoadCsvFile("Spoje.txt", ParseSpoje);
    }
    
    /// <summary>
    /// Načíta zastávky spojov zo súboru Zasspoje.txt
    /// </summary>
    public List<Zasspoje> LoadZasspoje()
    {
        return LoadCsvFile("Zasspoje.txt", ParseZasspoje);
    }
    
    /// <summary>
    /// Načíta časové kódy zo súboru Caskody.txt
    /// </summary>
    public List<Caskody> LoadCaskody()
    {
        return LoadCsvFile("Caskody.txt", ParseCaskody);
    }
    
    /// <summary>
    /// Načíta pevné kódy zo súboru Pevnykod.txt
    /// </summary>
    public List<Pevnykod> LoadPevnyKod()
    {
        return LoadCsvFile("Pevnykod.txt", ParsePevnyKod);
    }
    
    /// <summary>
    /// Načíta označníky zo súboru Oznacniky.txt
    /// </summary>
    public List<Oznacniky> LoadOznacniky()
    {
        return LoadCsvFile("Oznacniky.txt", ParseOznacniky);
    }
    
    // Pomocné metódy
    
    private List<T> LoadCsvFile<T>(string fileName, Func<Dictionary<string, string>, T> parser)
    {
        var filePath = Path.Combine(_jdfDirectoryPath, fileName);
        
        if (!File.Exists(filePath))
        {
            return new List<T>();
        }
        
        var results = new List<T>();
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false, // JDF súbory nemajú hlavičku
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null, // Ignoruj chýbajúce polia
            BadDataFound = null, // Ignoruj chybné dáta
            Delimiter = ",", // JDF používa čiarku ako oddeľovač
            Quote = '"' // JDF používa úvodzovky okolo hodnôt
        };
        
        using var reader = new StreamReader(filePath); 
        using var csv = new CsvReader(reader, config);
        
        // Prečítaj všetky riadky (bez hlavičky)
        while (csv.Read())
        {
            try
            {
                var row = new Dictionary<string, string>();
                
                // Prečítaj všetky polia v riadku podľa indexu
                int fieldCount = csv.Parser.Count;
                for (int i = 0; i < fieldCount; i++)
                {
                    var value = csv.GetField(i) ?? string.Empty;
                    // Odstráň úvodzovky, ak sú prítomné
                    value = value.Trim('"').Trim();
                    // Použij index ako kľúč
                    row[i.ToString()] = value;
                }
                
                var item = parser(row);
                results.Add(item);
            }
            catch (Exception ex)
            {
                // Ignoruj chybné riadky
                Console.Error.WriteLine($"Chyba pri parsovaní riadku v súbore {fileName}: {ex.Message}");
            }
        }
        
        return results;
    }
    
    private string GetValue(Dictionary<string, string> row, string key)
    {
        return row.TryGetValue(key, out var value) ? value.Trim() : string.Empty;
    }
    
    private int? ParseInt(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        
        return int.TryParse(value, out var result) ? result : null;
    }
    
    private int ParseIntRequired(string value, int defaultValue = 0)
    {
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;
        
        return int.TryParse(value, out var result) ? result : defaultValue;
    }
    
    private decimal? ParseDecimal(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        
        // JDF používa čiarku ako desatinný oddeľovač
        value = value.Replace(',', '.');
        return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : null;
    }
    
    private bool ParseBool(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;
        
        // Môže byť "1" alebo "0", alebo "true" alebo "false"
        if (value == "1" || value.Equals("true", StringComparison.OrdinalIgnoreCase))
            return true;
        
        return false;
    }
    
    private T? ParseEnum<T>(string value) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        
        if (int.TryParse(value, out var intValue))
        {
            if (Enum.IsDefined(typeof(T), intValue))
            {
                return (T)(object)intValue;
            }
        }
        
        return null;
    }
    
    private T ParseEnumRequired<T>(string value, T defaultValue = default) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;
        
        if (int.TryParse(value, out var intValue))
        {
            if (Enum.IsDefined(typeof(T), intValue))
            {
                return (T)(object)intValue;
            }
        }
        
        return defaultValue;
    }
    
    // Parsery pre jednotlivé entity
    
    private VerzeJDF ParseVerzeJDF(Dictionary<string, string> row)
    {
        return new VerzeJDF
        {
            VerziaJDF = GetValue(row, "0"),
            CisloDU = ParseInt(GetValue(row, "1")),
            OkresKraj = GetValue(row, "2"),
            IdentikaciaDat = GetValue(row, "3"),
            DatumVyrobyDat = GetValue(row, "4"),
            Meno = GetValue(row, "5")
        };
    }
    
    private Dopravci ParseDopravci(Dictionary<string, string> row)
    {
        return new Dopravci
        {
            IC = GetValue(row, "0"),
            DIC = GetValue(row, "1"),
            ObchodnéMeno = GetValue(row, "2"),
            DruhFirmy = ParseEnumRequired(GetValue(row, "3"), DruhFirmy.PravnickaOsoba),
            MenoFyzOsoby = GetValue(row, "4"),
            Sidlo = GetValue(row, "5"),
            TelefonSidlo = GetValue(row, "6"),
            TelefonDispecink = GetValue(row, "7"),
            TelefonInformace = GetValue(row, "8"),
            Fax = GetValue(row, "9"),
            Email = GetValue(row, "10"),
            Web = GetValue(row, "11"),
            RozlisenieDopravcu = ParseIntRequired(GetValue(row, "12"))
        };
    }
    
    private Zastavky ParseZastavky(Dictionary<string, string> row)
    {
        var zastavka = new Zastavky
        {
            Cislo = ParseIntRequired(GetValue(row, "0")),
            NazovObce = GetValue(row, "1"),
            CastObce = GetValue(row, "2"),
            BlizkeMiesto = GetValue(row, "3"),
            BlizkaObec = GetValue(row, "4"),
            Stat = GetValue(row, "5")
        };
        
        // Načítaj pevné kódy do poľa ako enum
        for (int i = 0; i < 6; i++)
        {
            var value = GetValue(row, $"{i + 6}");
            zastavka.PevneKody[i] = PevnyKodExtensions.ZoCisla(value);
        }
        
        return zastavka;
    }
    
    private Linky ParseLinky(Dictionary<string, string> row)
    {
        return new Linky
        {
            Cislo = ParseIntRequired(GetValue(row, "0")),
            Nazov = GetValue(row, "1"),
            IcDopravce = GetValue(row, "2"),
            Typ = ParseEnumRequired(GetValue(row, "3"), TypLinky.Mestska),
            DopravnyProstriedok = ParseEnumRequired(GetValue(row, "4"), DopravnyProstriedok.Autobus),
            ObjizdkovyJR = ParseBool(GetValue(row, "5")),
            SeskupenieSpojov = ParseBool(GetValue(row, "6")),
            PouzitieOznacnikov = ParseBool(GetValue(row, "7")),
            Rezerva = GetValue(row, "8"),
            CisloLicencie = GetValue(row, "9"),
            PlatnostLicencieOd = GetValue(row, "10"),
            PlatnostLicencieDo = GetValue(row, "11"),
            PlatnostJROd = GetValue(row, "12"),
            PlatnostJRDo = GetValue(row, "13"),
            RozlisenieDopravcu = ParseIntRequired(GetValue(row, "14")),
            RozlisenieLinky = ParseIntRequired(GetValue(row, "15"))
        };
    }
    
    private Zaslinky ParseZaslinky(Dictionary<string, string> row)
    {
        var zaslinka = new Zaslinky
        {
            CisloLinky = ParseIntRequired(GetValue(row, "0")),
            CisloTarifni = ParseIntRequired(GetValue(row, "1")),
            TarifniPasmo = GetValue(row, "2"),
            CisloZastavky = ParseIntRequired(GetValue(row, "3")),
            PriemernaDoba = GetValue(row, "4"),
            RozlisenieLinky = ParseIntRequired(GetValue(row, "8"))
        };
        
        // Načítaj pevné kódy do poľa ako enum
        for (int i = 0; i < 3; i++)
        {
            var value = GetValue(row, $"{i + 5}");
            zaslinka.PevneKody[i] = PevnyKodExtensions.ZoCisla(value);
        }
        
        return zaslinka;
    }
    
    private Spoje ParseSpoje(Dictionary<string, string> row)
    {
        var spoj = new Spoje
        {
            CisloLinky = ParseIntRequired(GetValue(row, "0")),
            Cislo = ParseIntRequired(GetValue(row, "1")),
            KodSkupinySpoju = ParseInt(GetValue(row, "12")),
            RozliseniLinky = ParseIntRequired(GetValue(row, "13"))
        };
        
        // Načítaj pevné kódy do poľa ako enum
        for (int i = 0; i < 10; i++)
        {
            var value = GetValue(row, $"{i + 2}");
            spoj.PevneKody[i] = PevnyKodExtensions.ZoCisla(ParseInt(value));
        }
        
        return spoj;
    }
    
    private Zasspoje ParseZasspoje(Dictionary<string, string> row)
    {
        var zasspoj = new Zasspoje
        {
            CisloLinky = ParseIntRequired(GetValue(row, "0")),
            CisloSpoje = ParseIntRequired(GetValue(row, "1")),
            CisloTarifni = ParseIntRequired(GetValue(row, "2")),
            CisloZastavky = ParseIntRequired(GetValue(row, "3")),
            KodOznacniku = ParseInt(GetValue(row, "4")),
            CisloStanoviste = GetValue(row, "5"),
            Kilometry = ParseDecimal(GetValue(row, "8")),
            CasPrichodu = GetValue(row, "9"),
            CasOdchodu = GetValue(row, "10"),
            RozlisenieLinky = ParseIntRequired(GetValue(row, "11"))
        };
        
        // Načítaj pevné kódy do poľa ako enum
        for (int i = 0; i < 2; i++)
        {
            var value = GetValue(row, $"{i + 6}");
            zasspoj.PevneKody[i] = PevnyKodExtensions.ZoCisla(value);
        }
        
        return zasspoj;
    }
    
    private Caskody ParseCaskody(Dictionary<string, string> row)
    {
        return new Caskody
        {
            CisloLinky = ParseIntRequired(GetValue(row, "0")),
            CisloSpoje = ParseIntRequired(GetValue(row, "1")),
            Cislo = ParseIntRequired(GetValue(row, "2")),
            Oznacenie = ParseIntRequired(GetValue(row, "3")),
            Typ = ParseEnum<TypCasKod>(GetValue(row, "4")),
            DatumOd = GetValue(row, "5"),
            DatumDo = GetValue(row, "6"),
            Poznamka = GetValue(row, "7"),
            RozlisenieLinky = ParseIntRequired(GetValue(row, "8"))
        };
    }
    
    private Pevnykod ParsePevnyKod(Dictionary<string, string> row)
    {
        return new Pevnykod
        {
            Cislo = GetValue(row, "0"),
            Oznacenie = GetValue(row, "1"),
            Rezerva = GetValue(row, "2")
        };
    }
    
    private Oznacniky ParseOznacniky(Dictionary<string, string> row)
    {
        return new Oznacniky
        {
            CisloZastavky = ParseIntRequired(GetValue(row, "0")),
            KodOznacniku = ParseIntRequired(GetValue(row, "1")),
            Nazov = GetValue(row, "2"),
            SmerPopis = GetValue(row, "3"),
            Stanoviste = GetValue(row, "4"),
            Rezerva = GetValue(row, "5")
        };
    }
}