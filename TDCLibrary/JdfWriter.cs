using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TDCLibrary.JdfModel;

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
    
    private void WriteCsv<T>(string fileName, IEnumerable<T> records, Action<CsvWriter, T> map, string[] header)
    {
        var path = Path.Combine(_directory, fileName);
        using var writer = new StreamWriter(path, false, System.Text.Encoding.GetEncoding("windows-1250")); // JDF používa windows-1250
        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            ShouldQuote = args => true, // JDF používa úvodzovky okolo všetkých polí
            Delimiter = ",", // JDF používa čiarku ako oddeľovač
            Quote = '"'
        };
        using var csv = new CsvWriter(writer, cfg);
        
        // Zapíš hlavičku
        foreach (var h in header)
        {
            csv.WriteField(h);
        }
        csv.NextRecord();
        
        // Zapíš záznamy
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
        }, new[] { "VerziaJDF", "CisloDU", "OkresKraj", "IdentikaciaDat", "DatumVyrobyDat", "Meno" });
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
        }, new[] { "IC", "DIC", "ObchodneMeno", "DruhFirmy", "MenoFyzOsoby", "Sidlo", "TelefonSidlo", 
                   "TelefonDispecink", "TelefonInformace", "Fax", "Email", "Web", "RozlisenieDopravcu" });
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
            csv.WriteField(z.PevnyKod1 ?? "");
            csv.WriteField(z.PevnyKod2 ?? "");
            csv.WriteField(z.PevnyKod3 ?? "");
            csv.WriteField(z.PevnyKod4 ?? "");
            csv.WriteField(z.PevnyKod5 ?? "");
            csv.WriteField(z.PevnyKod6 ?? "");
        }, new[] { "CisloZastavky", "NazovObce", "CastObce", "BlizkeMiesto", "BlizkaObec", "Stat", 
                   "PevnyKod1", "PevnyKod2", "PevnyKod3", "PevnyKod4", "PevnyKod5", "PevnyKod6" });
    }
    
    private void WriteLinky(List<Linky> linky)
    {
        WriteCsv("Linky.txt", linky, (csv, l) =>
        {
            csv.WriteField(l.Cislo.ToString());
            csv.WriteField(l.Nazov);
            csv.WriteField(l.IcDopravce);
            csv.WriteField(((int)l.Typ).ToString());
            csv.WriteField(((int)l.DopravnyProstriedok).ToString());
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
        }, new[] { "Cislo", "Nazov", "IcDopravce", "Typ", "DopravnyProstriedok", "ObjizdkovyJR", 
                   "SeskupenieSpojov", "PouzitieOznacnikov", "Rezerva", "CisloLicencie", 
                   "PlatnostLicencieOd", "PlatnostLicencieDo", "PlatnostJROd", "PlatnostJRDo", 
                   "RozlisenieDopravcu", "RozlisenieLinky" });
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
            csv.WriteField(z.PevnyKod1 ?? "");
            csv.WriteField(z.PevnyKod2 ?? "");
            csv.WriteField(z.PevnyKod3 ?? "");
            csv.WriteField(z.RozlisenieLinky.ToString());
        }, new[] { "CisloLinky", "CisloTarifni", "TarifniPasmo", "CisloZastavky", "PriemernaDoba", 
                   "PevnyKod1", "PevnyKod2", "PevnyKod3", "RozlisenieLinky" });
    }
    
    private void WriteSpoje(List<Spoje> spoje)
    {
        WriteCsv("Spoje.txt", spoje, (csv, s) =>
        {
            csv.WriteField(s.CisloLinky.ToString());
            csv.WriteField(s.Cislo.ToString());
            csv.WriteField(s.PevnyKod1?.ToString() ?? "");
            csv.WriteField(s.PevnyKod2?.ToString() ?? "");
            csv.WriteField(s.PevnyKod3?.ToString() ?? "");
            csv.WriteField(s.PevnyKod4?.ToString() ?? "");
            csv.WriteField(s.PevnyKod5?.ToString() ?? "");
            csv.WriteField(s.PevnyKod6?.ToString() ?? "");
            csv.WriteField(s.PevnyKod7?.ToString() ?? "");
            csv.WriteField(s.PevnyKod8?.ToString() ?? "");
            csv.WriteField(s.PevnyKod9?.ToString() ?? "");
            csv.WriteField(s.PevnyKod10?.ToString() ?? "");
            csv.WriteField(s.KodSkupinySpoju?.ToString() ?? "");
            csv.WriteField(s.RozliseniLinky.ToString());
        }, new[] { "CisloLinky", "CisloSpoje", "PevnyKod1", "PevnyKod2", "PevnyKod3", "PevnyKod4", 
                   "PevnyKod5", "PevnyKod6", "PevnyKod7", "PevnyKod8", "PevnyKod9", "PevnyKod10", 
                   "KodSkupinySpoju", "RozliseniLinky" });
    }
    
    private void WriteZasspoje(List<Zasspoje> zasspoje)
    {
        WriteCsv("Zasspoje.txt", zasspoje, (csv, z) =>
        {
            csv.WriteField(z.CisloLinky.ToString());
            csv.WriteField(z.CisloSpoje.ToString());
            csv.WriteField(z.CisloTarifni.ToString());
            csv.WriteField(z.CisloZastavky.ToString());
            csv.WriteField(z.KodOznacniku?.ToString() ?? "");
            csv.WriteField(z.CisloStanoviste ?? "");
            csv.WriteField(z.PevnyKod1 ?? "");
            csv.WriteField(z.PevnyKod2 ?? "");
            csv.WriteField(z.Kilometry?.ToString(CultureInfo.InvariantCulture).Replace('.', ',') ?? "");
            csv.WriteField(z.CasPrichodu);
            csv.WriteField(z.CasOdchodu ?? "");
            csv.WriteField(z.RozlisenieLinky.ToString());
        }, new[] { "CisloLinky", "CisloSpoje", "CisloTarifni", "CisloZastavky", "KodOznacniku", 
                   "CisloStanoviste", "PevnyKod1", "PevnyKod2", "Kilometry", "CasPrichodu", 
                   "CasOdchodu", "RozlisenieLinky" });
    }
    
    private void WriteCaskody(List<Caskody> caskody)
    {
        WriteCsv("Caskody.txt", caskody, (csv, c) =>
        {
            csv.WriteField(c.CisloLinky.ToString());
            csv.WriteField(c.CisloSpoje.ToString());
            csv.WriteField(c.Cislo.ToString());
            csv.WriteField(c.Oznacenie);
            csv.WriteField(c.Typ?.ToString() ?? "");
            csv.WriteField(c.DatumOd ?? "");
            csv.WriteField(c.DatumDo ?? "");
            csv.WriteField(c.Poznamka ?? "");
            csv.WriteField(c.RozlisenieLinky.ToString());
        }, new[] { "CisloLinky", "CisloSpoje", "Cislo", "Oznacenie", "Typ", "DatumOd", "DatumDo", 
                   "Poznamka", "RozlisenieLinky" });
    }
    
    private void WritePevnyKod(List<Pevnykod> pevnykod)
    {
        WriteCsv("Pevnykod.txt", pevnykod, (csv, p) =>
        {
            csv.WriteField(p.Cislo);
            csv.WriteField(p.Oznacenie);
            csv.WriteField(p.Rezerva ?? "");
        }, new[] { "Cislo", "Oznacenie", "Rezerva" });
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
        }, new[] { "CisloZastavky", "KodOznacniku", "Nazov", "SmerPopis", "Stanoviste", "Rezerva" });
    }
}