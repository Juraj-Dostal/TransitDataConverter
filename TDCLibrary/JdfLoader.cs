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
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null, // Ignoruj chýbajúce polia
            BadDataFound = null, // Ignoruj chybné dáta
            Delimiter = ",", // JDF používa čiarku ako oddeľovač
            Quote = '"' // JDF používa úvodzovky okolo hodnôt
        };
        
        using var reader = new StreamReader(filePath, System.Text.Encoding.GetEncoding("windows-1250")); // JDF používa windows-1250
        using var csv = new CsvReader(reader, config);
        
        // Prečítaj hlavičku
        csv.Read();
        csv.ReadHeader();
        var headers = csv.HeaderRecord;
        
        if (headers == null || headers.Length == 0)
        {
            return results;
        }
        
        // Prečítaj všetky riadky
        while (csv.Read())
        {
            try
            {
                var row = new Dictionary<string, string>();
                
                foreach (var header in headers)
                {
                    var value = csv.GetField(header) ?? string.Empty;
                    // Odstráň úvodzovky, ak sú prítomné
                    value = value.Trim('"').Trim();
                    row[header] = value;
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
            VerziaJDF = GetValue(row, "VerziaJDF"),
            CisloDU = ParseInt(GetValue(row, "CisloDU")),
            OkresKraj = GetValue(row, "OkresKraj"),
            IdentikaciaDat = GetValue(row, "IdentikaciaDat"),
            DatumVyrobyDat = GetValue(row, "DatumVyrobyDat"),
            Meno = GetValue(row, "Meno")
        };
    }
    
    private Dopravci ParseDopravci(Dictionary<string, string> row)
    {
        return new Dopravci
        {
            IC = GetValue(row, "IC"),
            DIC = GetValue(row, "DIC"),
            ObchodnéMeno = GetValue(row, "ObchodneMeno"),
            DruhFirmy = ParseEnumRequired(GetValue(row, "DruhFirmy"), DruhFirmy.PravnickaOsoba),
            MenoFyzOsoby = GetValue(row, "MenoFyzOsoby"),
            Sidlo = GetValue(row, "Sidlo"),
            TelefonSidlo = GetValue(row, "TelefonSidlo"),
            TelefonDispecink = GetValue(row, "TelefonDispecink"),
            TelefonInformace = GetValue(row, "TelefonInformace"),
            Fax = GetValue(row, "Fax"),
            Email = GetValue(row, "Email"),
            Web = GetValue(row, "Web"),
            RozlisenieDopravcu = ParseIntRequired(GetValue(row, "RozlisenieDopravcu"))
        };
    }
    
    private Zastavky ParseZastavky(Dictionary<string, string> row)
    {
        var zastavka = new Zastavky
        {
            Cislo = ParseIntRequired(GetValue(row, "CisloZastavky")),
            NazovObce = GetValue(row, "NazovObce"),
            CastObce = GetValue(row, "CastObce"),
            BlizkeMiesto = GetValue(row, "BlizkeMiesto"),
            BlizkaObec = GetValue(row, "BlizkaObec"),
            Stat = GetValue(row, "Stat")
        };
        
        // Načítaj pevné kódy do poľa ako enum
        for (int i = 0; i < 6; i++)
        {
            var value = GetValue(row, $"PevnyKod{i + 1}");
            zastavka.PevneKody[i] = PevnyKodExtensions.ZoCisla(value);
        }
        
        return zastavka;
    }
    
    private Linky ParseLinky(Dictionary<string, string> row)
    {
        return new Linky
        {
            Cislo = ParseIntRequired(GetValue(row, "Cislo")),
            Nazov = GetValue(row, "Nazov"),
            IcDopravce = GetValue(row, "IcDopravce"),
            Typ = ParseEnumRequired(GetValue(row, "Typ"), TypLinky.Mestska),
            DopravnyProstriedok = ParseEnumRequired(GetValue(row, "DopravnyProstriedok"), DopravnyProstriedok.Autobus),
            ObjizdkovyJR = ParseBool(GetValue(row, "ObjizdkovyJR")),
            SeskupenieSpojov = ParseBool(GetValue(row, "SeskupenieSpojov")),
            PouzitieOznacnikov = ParseBool(GetValue(row, "PouzitieOznacnikov")),
            Rezerva = GetValue(row, "Rezerva"),
            CisloLicencie = GetValue(row, "CisloLicencie"),
            PlatnostLicencieOd = GetValue(row, "PlatnostLicencieOd"),
            PlatnostLicencieDo = GetValue(row, "PlatnostLicencieDo"),
            PlatnostJROd = GetValue(row, "PlatnostJROd"),
            PlatnostJRDo = GetValue(row, "PlatnostJRDo"),
            RozlisenieDopravcu = ParseIntRequired(GetValue(row, "RozlisenieDopravcu")),
            RozlisenieLinky = ParseIntRequired(GetValue(row, "RozlisenieLinky"))
        };
    }
    
    private Zaslinky ParseZaslinky(Dictionary<string, string> row)
    {
        var zaslinka = new Zaslinky
        {
            CisloLinky = ParseIntRequired(GetValue(row, "CisloLinky")),
            CisloTarifni = ParseIntRequired(GetValue(row, "CisloTarifni")),
            TarifniPasmo = GetValue(row, "TarifniPasmo"),
            CisloZastavky = ParseIntRequired(GetValue(row, "CisloZastavky")),
            PriemernaDoba = GetValue(row, "PriemernaDoba"),
            RozlisenieLinky = ParseIntRequired(GetValue(row, "RozlisenieLinky"))
        };
        
        // Načítaj pevné kódy do poľa ako enum
        for (int i = 0; i < 3; i++)
        {
            var value = GetValue(row, $"PevnyKod{i + 1}");
            zaslinka.PevneKody[i] = PevnyKodExtensions.ZoCisla(value);
        }
        
        return zaslinka;
    }
    
    private Spoje ParseSpoje(Dictionary<string, string> row)
    {
        var spoj = new Spoje
        {
            CisloLinky = ParseIntRequired(GetValue(row, "CisloLinky")),
            Cislo = ParseIntRequired(GetValue(row, "CisloSpoje")),
            KodSkupinySpoju = ParseInt(GetValue(row, "KodSkupinySpoju")),
            RozliseniLinky = ParseIntRequired(GetValue(row, "RozliseniLinky"))
        };
        
        // Načítaj pevné kódy do poľa ako enum
        for (int i = 0; i < 10; i++)
        {
            var value = GetValue(row, $"PevnyKod{i + 1}");
            spoj.PevneKody[i] = PevnyKodExtensions.ZoCisla(ParseInt(value));
        }
        
        return spoj;
    }
    
    private Zasspoje ParseZasspoje(Dictionary<string, string> row)
    {
        var zasspoj = new Zasspoje
        {
            CisloLinky = ParseIntRequired(GetValue(row, "CisloLinky")),
            CisloSpoje = ParseIntRequired(GetValue(row, "CisloSpoje")),
            CisloTarifni = ParseIntRequired(GetValue(row, "CisloTarifni")),
            CisloZastavky = ParseIntRequired(GetValue(row, "CisloZastavky")),
            KodOznacniku = ParseInt(GetValue(row, "KodOznacniku")),
            CisloStanoviste = GetValue(row, "CisloStanoviste"),
            Kilometry = ParseDecimal(GetValue(row, "Kilometry")),
            CasPrichodu = GetValue(row, "CasPrichodu"),
            CasOdchodu = GetValue(row, "CasOdchodu"),
            RozlisenieLinky = ParseIntRequired(GetValue(row, "RozlisenieLinky"))
        };
        
        // Načítaj pevné kódy do poľa ako enum
        for (int i = 0; i < 2; i++)
        {
            var value = GetValue(row, $"PevnyKod{i + 1}");
            zasspoj.PevneKody[i] = PevnyKodExtensions.ZoCisla(value);
        }
        
        return zasspoj;
    }
    
    private Caskody ParseCaskody(Dictionary<string, string> row)
    {
        return new Caskody
        {
            CisloLinky = ParseIntRequired(GetValue(row, "CisloLinky")),
            CisloSpoje = ParseIntRequired(GetValue(row, "CisloSpoje")),
            Cislo = ParseIntRequired(GetValue(row, "Cislo")),
            Oznacenie = GetValue(row, "Oznacenie"),
            Typ = ParseInt(GetValue(row, "Typ")),
            DatumOd = GetValue(row, "DatumOd"),
            DatumDo = GetValue(row, "DatumDo"),
            Poznamka = GetValue(row, "Poznamka"),
            RozlisenieLinky = ParseIntRequired(GetValue(row, "RozlisenieLinky"))
        };
    }
    
    private Pevnykod ParsePevnyKod(Dictionary<string, string> row)
    {
        return new Pevnykod
        {
            Cislo = GetValue(row, "Cislo"),
            Oznacenie = GetValue(row, "Oznacenie"),
            Rezerva = GetValue(row, "Rezerva")
        };
    }
    
    private Oznacniky ParseOznacniky(Dictionary<string, string> row)
    {
        return new Oznacniky
        {
            CisloZastavky = ParseIntRequired(GetValue(row, "CisloZastavky")),
            KodOznacniku = ParseIntRequired(GetValue(row, "KodOznacniku")),
            Nazov = GetValue(row, "Nazov"),
            SmerPopis = GetValue(row, "SmerPopis"),
            Stanoviste = GetValue(row, "Stanoviste"),
            Rezerva = GetValue(row, "Rezerva")
        };
    }
}