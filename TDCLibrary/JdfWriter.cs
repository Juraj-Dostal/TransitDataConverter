using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TDCLibrary.JdfModel;
using TDCLibrary.ConvertorModel;

namespace TDCLibrary;

/// <summary>
/// Zapisuje JDF (Jednotný Dátový Formát) dáta späť do CSV súborov v zadanom adresári.
/// Zapisuje všetky JDF súbory podľa špecifikácie formátu.
/// </summary>
public class JdfWriter
{
    private readonly string _directory;
    
    /// <summary>
    /// Inicializuje nový zapisovač JDF dát.
    /// </summary>
    /// <param name="directory">Cesta k adresáru, kam sa majú zapísať JDF súbory</param>
    public JdfWriter(string directory)
    {
        _directory = directory ?? throw new ArgumentNullException(nameof(directory));
        
        if (!Directory.Exists(_directory))
        {
            Directory.CreateDirectory(_directory);
        }
    }
    
    /// <summary>
    /// Zapíše všetky JDF dáta do súborov.
    /// </summary>
    /// <param name="data">JDF dáta na zápis</param>
    public void WriteAll(JdfData data)
    {
        if (data.VerzeJDF != null)
            WriteVerzeJDF(data.VerzeJDF);
        
        WriteDopravci(data.Dopravci);
        WriteZastavky(data.Zastavky);
        WriteLinky(data.Linky);
        WriteZaslinky(data.Zaslinky);
        WriteSpoje(data.Spoje);
        WriteZasspoje(data.Zasspoje);
        WriteCaskody(data.Caskody);
        WritePevnyKod(data.PevnyKod);
        
        if (data.Oznacniky.Count > 0)
            WriteOznacniky(data.Oznacniky);
    }
    
    private void WriteCsv<T>(string fileName, IEnumerable<T> records, Action<CsvWriter, T> map)
    {
        var path = Path.Combine(_directory, fileName);
        using var writer = new StreamWriter(path, false); // JDF používa windows-1250
        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            ShouldQuote = args => true, // JDF používa úvodzovky okolo všetkých polí
            Delimiter = ",", // JDF používa čiarku ako oddeľovač
            Quote = '"'
        };
        using var csv = new CsvWriter(writer, cfg);
        
        // Zapíš záznamy (bez hlavičky)
        foreach (var r in records)
        {
            map(csv, r);
            csv.NextRecord();
        }
    }
    
    private void WriteVerzeJDF(VerzeJDF verze)
    {
        WriteCsv("VerzeJDF.txt", new[] { verze }, (csv, v) =>
        {
            csv.WriteField(v.VerziaJDF);
            csv.WriteField(v.CisloDU?.ToString() ?? "");
            csv.WriteField(v.OkresKraj ?? "");
            csv.WriteField(v.IdentikaciaDat ?? "");
            csv.WriteField(v.DatumVyrobyDat);
            csv.WriteField(v.Meno ?? "");
        });
    }
    
    private void WriteDopravci(List<Dopravci> dopravci)
    {
        WriteCsv("Dopravci.txt", dopravci, (csv, d) =>
        {
            csv.WriteField(d.IC);
            csv.WriteField(d.DIC ?? "");
            csv.WriteField(d.ObchodnéMeno);
            csv.WriteField(((int)d.DruhFirmy).ToString());
            csv.WriteField(d.MenoFyzOsoby ?? "");
            csv.WriteField(d.Sidlo);
            csv.WriteField(d.TelefonSidlo);
            csv.WriteField(d.TelefonDispecink ?? "");
            csv.WriteField(d.TelefonInformace ?? "");
            csv.WriteField(d.Fax ?? "");
            csv.WriteField(d.Email ?? "");
            csv.WriteField(d.Web ?? "");
            csv.WriteField(d.RozlisenieDopravcu.ToString());
        });
    }
    
    private void WriteZastavky(List<Zastavky> zastavky)
    {
        WriteCsv("Zastavky.txt", zastavky, (csv, z) =>
        {
            csv.WriteField(z.Cislo.ToString());
            csv.WriteField(z.NazovObce);
            csv.WriteField(z.CastObce ?? "");
            csv.WriteField(z.BlizkeMiesto ?? "");
            csv.WriteField(z.BlizkaObec ?? "");
            csv.WriteField(z.Stat);
            // Zapíš pevné kódy z poľa ako 5-miestne čísla
            for (int i = 0; i < 6; i++)
            {
                csv.WriteField(z.PevneKody[i].HasValue ? PevnyKodExtensions.DajCislo(z.PevneKody[i].Value) : "");
            }
        });
    }
    
    private void WriteLinky(List<Linky> linky)
    {
        WriteCsv("Linky.txt", linky, (csv, l) =>
        {
            csv.WriteField(l.Cislo.ToString("D6"));
            csv.WriteField(l.Nazov);
            csv.WriteField(l.IcDopravce);
            csv.WriteField(EnumExtension.GetDescription(l.Typ));
            csv.WriteField(EnumExtension.GetDescription(l.DopravnyProstriedok));
            csv.WriteField(l.ObjizdkovyJR ? "1" : "0");
            csv.WriteField(l.SeskupenieSpojov ? "1" : "0");
            csv.WriteField(l.PouzitieOznacnikov ? "1" : "0");
            csv.WriteField(l.Rezerva ?? "");
            csv.WriteField(l.CisloLicencie ?? "");
            csv.WriteField(l.PlatnostLicencieOd ?? "");
            csv.WriteField(l.PlatnostLicencieDo ?? "");
            csv.WriteField(l.PlatnostJROd);
            csv.WriteField(l.PlatnostJRDo);
            csv.WriteField(l.RozlisenieDopravcu.ToString());
            csv.WriteField(l.RozlisenieLinky.ToString());
        });
    }
    
    private void WriteZaslinky(List<Zaslinky> zaslinky)
    {
        WriteCsv("Zaslinky.txt", zaslinky, (csv, z) =>
        {
            csv.WriteField(z.CisloLinky.ToString());
            csv.WriteField(z.CisloTarifni.ToString());
            csv.WriteField(z.TarifniPasmo ?? "");
            csv.WriteField(z.CisloZastavky.ToString());
            csv.WriteField(z.PriemernaDoba ?? "");
            // Zapíš pevné kódy z poľa ako 5-miestne čísla
            for (int i = 0; i < 3; i++)
            {
                csv.WriteField(z.PevneKody[i].HasValue ? PevnyKodExtensions.DajCislo(z.PevneKody[i].Value) : "");
            }
            csv.WriteField(z.RozlisenieLinky.ToString());
        });
    }
    
    private void WriteSpoje(List<Spoje> spoje)
    {
        WriteCsv("Spoje.txt", spoje, (csv, s) =>
        {
            csv.WriteField(s.CisloLinky.ToString("D6"));
            csv.WriteField(s.Cislo.ToString());
            // Zapíš pevné kódy z poľa ako 5-miestne čísla
            for (int i = 0; i < 10; i++)
            {
                csv.WriteField(s.PevneKody[i].HasValue ? PevnyKodExtensions.DajCislo(s.PevneKody[i].Value) : "");
            }
            csv.WriteField(s.KodSkupinySpoju?.ToString() ?? "");
            csv.WriteField(s.RozliseniLinky.ToString());
        });
    }
    
    private void WriteZasspoje(List<Zasspoje> zasspoje)
    {
        WriteCsv("Zasspoje.txt", zasspoje, (csv, z) =>
        {
            csv.WriteField(z.CisloLinky.ToString("D6"));
            csv.WriteField(z.CisloSpoje.ToString());
            csv.WriteField(z.CisloTarifni.ToString());
            csv.WriteField(z.CisloZastavky.ToString());
            csv.WriteField(z.KodOznacniku?.ToString() ?? "");
            csv.WriteField(z.CisloStanoviste ?? "");
            // Zapíš pevné kódy z poľa ako 5-miestne čísla
            for (int i = 0; i < 2; i++)
            {
                csv.WriteField(z.PevneKody[i].HasValue ? PevnyKodExtensions.DajCislo(z.PevneKody[i].Value) : "");
            }
            csv.WriteField(z.Kilometry?.ToString(CultureInfo.InvariantCulture).Replace('.', ',') ?? "");
            csv.WriteField(z.CasPrichodu);
            csv.WriteField(z.CasOdchodu ?? "");
            csv.WriteField(z.RozlisenieLinky.ToString());
        });
    }
    
    private void WriteCaskody(List<Caskody> caskody)
    {
        WriteCsv("Caskody.txt", caskody, (csv, c) =>
        {
            csv.WriteField(c.CisloLinky.ToString("D6"));
            csv.WriteField(c.CisloSpoje.ToString());
            csv.WriteField(c.Cislo.ToString());
            csv.WriteField(c.Oznacenie.ToString());
            csv.WriteField(c.Typ.HasValue ? ((int)c.Typ.Value).ToString() : "");
            csv.WriteField(c.DatumOd ?? "");
            csv.WriteField(c.DatumDo ?? "");
            csv.WriteField(c.Poznamka ?? "");
            csv.WriteField(c.RozlisenieLinky.ToString());
        });
    }
    
    private void WritePevnyKod(List<Pevnykod> pevnykod)
    {
        WriteCsv("Pevnykod.txt", pevnykod, (csv, p) =>
        {
            csv.WriteField(p.Cislo);
            csv.WriteField(p.Oznacenie);
            csv.WriteField(p.Rezerva ?? "");
        });
    }
    
    private void WriteOznacniky(List<Oznacniky> oznacniky)
    {
        WriteCsv("Oznacniky.txt", oznacniky, (csv, o) =>
        {
            csv.WriteField(o.CisloZastavky.ToString());
            csv.WriteField(o.KodOznacniku.ToString());
            csv.WriteField(o.Nazov ?? "");
            csv.WriteField(o.SmerPopis ?? "");
            csv.WriteField(o.Stanoviste ?? "");
            csv.WriteField(o.Rezerva ?? "");
        });
    }
}