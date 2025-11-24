using TDCLibrary.JdfModel;
using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary.Examples;

/// <summary>
/// Príklady použitia JdfLoader a JdfWriter
/// </summary>
public static class JdfLoaderExamples
{
    /// <summary>
    /// Príklad načítania všetkých JDF dát
    /// </summary>
    public static void LoadAllExample()
    {
        Console.WriteLine("=== Príklad načítania všetkých JDF dát ===");
        
        var loader = new JdfLoader("/cesta/k/jdf/adresaru");
        var data = loader.LoadAll();
        
        Console.WriteLine($"Verzia JDF: {data.VerzeJDF?.VerziaJDF}");
        Console.WriteLine($"Dátum výroby: {data.VerzeJDF?.DatumVyrobyDat}");
        Console.WriteLine($"Počet dopravcov: {data.Dopravci.Count}");
        Console.WriteLine($"Počet zastávok: {data.Zastavky.Count}");
        Console.WriteLine($"Počet liniek: {data.Linky.Count}");
        Console.WriteLine($"Počet spojov: {data.Spoje.Count}");
    }
    
    /// <summary>
    /// Príklad načítania jednotlivých súborov
    /// </summary>
    public static void LoadIndividualFilesExample()
    {
        Console.WriteLine("=== Príklad načítania jednotlivých súborov ===");
        
        var loader = new JdfLoader("/cesta/k/jdf/adresaru");
        
        // Načítaj len dopravcov
        var dopravci = loader.LoadDopravci();
        Console.WriteLine($"Načítaných dopravcov: {dopravci.Count}");
        
        foreach (var dopravca in dopravci)
        {
            Console.WriteLine($"  IČ: {dopravca.IC}, Názov: {dopravca.ObchodnéMeno}");
        }
        
        // Načítaj len zastávky
        var zastavky = loader.LoadZastavky();
        Console.WriteLine($"\nNačítaných zastávok: {zastavky.Count}");
        
        foreach (var zastavka in zastavky.Take(5))
        {
            Console.WriteLine($"  Číslo: {zastavka.Cislo}, Obec: {zastavka.NazovObce}, Štát: {zastavka.Stat}");
        }
    }
    
    /// <summary>
    /// Príklad vytvorenia jednoduchého JDF datasetu
    /// </summary>
    public static void CreateSimpleDatasetExample()
    {
        Console.WriteLine("=== Príklad vytvorenia jednoduchého JDF datasetu ===");
        
        var data = new JdfData
        {
            VerzeJDF = new VerzeJDF
            {
                VerziaJDF = "1.10",
                DatumVyrobyDat = "15032024",
                Meno = "Testovací dataset"
            },
            
            Dopravci = new List<Dopravci>
            {
                new Dopravci
                {
                    IC = "12345678",
                    ObchodnéMeno = "Mestská doprava a.s.",
                    DruhFirmy = DruhFirmy.PravnickaOsoba,
                    Sidlo = "Hlavná 1, Bratislava",
                    TelefonSidlo = "+421212345678",
                    RozlisenieDopravcu = 1
                }
            },
            
            Zastavky = new List<Zastavky>
            {
                new Zastavky
                {
                    Cislo = 1,
                    NazovObce = "Bratislava",
                    CastObce = "Staré Mesto",
                    BlizkeMiesto = "Hlavná stanica",
                    BlizkaObec = "Bratislava",
                    Stat = "SK"
                },
                new Zastavky
                {
                    Cislo = 2,
                    NazovObce = "Bratislava",
                    CastObce = "Ružinov",
                    BlizkeMiesto = "Zimný štadión",
                    BlizkaObec = "Bratislava",
                    Stat = "SK"
                }
            },
            
            Linky = new List<Linky>
            {
                new Linky
                {
                    Cislo = 4,
                    Nazov = "Dúbravka - Karlova Ves",
                    IcDopravce = "12345678",
                    Typ = TypLinky.Mestska,
                    DopravnyProstriedok = DopravnyProstriedok.Elektricka,
                    ObjizdkovyJR = false,
                    SeskupenieSpojov = false,
                    PouzitieOznacnikov = true,
                    PlatnostJROd = "01012024",
                    PlatnostJRDo = "31122024",
                    RozlisenieDopravcu = 1,
                    RozlisenieLinky = 1
                }
            },
            
            Zaslinky = new List<Zaslinky>
            {
                new Zaslinky
                {
                    CisloLinky = 4,
                    CisloTarifni = 1,
                    CisloZastavky = 1,
                    RozlisenieLinky = 1
                },
                new Zaslinky
                {
                    CisloLinky = 4,
                    CisloTarifni = 2,
                    CisloZastavky = 2,
                    PriemernaDoba = "3",
                    RozlisenieLinky = 1
                }
            },
            
            Spoje = new List<Spoje>
            {
                new Spoje
                {
                    CisloLinky = 4,
                    Cislo = 1,
                    RozliseniLinky = 1
                }
            },
            
            Zasspoje = new List<Zasspoje>
            {
                new Zasspoje
                {
                    CisloLinky = 4,
                    CisloSpoje = 1,
                    CisloTarifni = 1,
                    CisloZastavky = 1,
                    Kilometry = 0.0m,
                    CasPrichodu = "0600",
                    RozlisenieLinky = 1
                },
                new Zasspoje
                {
                    CisloLinky = 4,
                    CisloSpoje = 1,
                    CisloTarifni = 2,
                    CisloZastavky = 2,
                    Kilometry = 1.5m,
                    CasPrichodu = "0603",
                    RozlisenieLinky = 1
                }
            },
            
            Caskody = new List<Caskody>
            {
                new Caskody
                {
                    CisloLinky = 4,
                    CisloSpoje = 1,
                    Cislo = 1,
                    Oznacenie = "X",
                    Typ = 1,
                    DatumOd = "01012024",
                    DatumDo = "31122024",
                    Poznamka = "Pracovné dni",
                    RozlisenieLinky = 1
                }
            },
            
            PevnyKod = new List<Pevnykod>
            {
                new Pevnykod
                {
                    Cislo = "1",
                    Oznacenie = "#",
                    Rezerva = "Bezbariérový"
                },
                new Pevnykod
                {
                    Cislo = "2",
                    Oznacenie = "K",
                    Rezerva = "Klimatizácia"
                }
            }
        };
        
        Console.WriteLine("Dataset vytvorený.");
        Console.WriteLine($"Počet dopravcov: {data.Dopravci.Count}");
        Console.WriteLine($"Počet zastávok: {data.Zastavky.Count}");
        Console.WriteLine($"Počet liniek: {data.Linky.Count}");
    }
    
    /// <summary>
    /// Príklad zápisu JDF dát do súborov
    /// </summary>
    public static void WriteDataExample()
    {
        Console.WriteLine("=== Príklad zápisu JDF dát ===");
        
        // Vytvor dataset (používa sa dataset z CreateSimpleDatasetExample)
        var data = new JdfData();
        // ... naplň data ...
        
        var writer = new JdfWriter("/cesta/k/vystupnemu/adresaru");
        writer.WriteAll(data);
        
        Console.WriteLine("JDF dáta boli zapísané do súborov.");
    }
    
    /// <summary>
    /// Príklad načítania a úpravy JDF dát
    /// </summary>
    public static void LoadAndModifyExample()
    {
        Console.WriteLine("=== Príklad načítania a úpravy JDF dát ===");
        
        // Načítaj dáta
        var loader = new JdfLoader("/cesta/k/jdf/adresaru");
        var data = loader.LoadAll();
        
        Console.WriteLine($"Pôvodný počet liniek: {data.Linky.Count}");
        
        // Pridaj novú linku
        var novaLinka = new Linky
        {
            Cislo = 99,
            Nazov = "Nová linka",
            IcDopravce = data.Dopravci[0].IC,
            Typ = TypLinky.Mestska,
            DopravnyProstriedok = DopravnyProstriedok.Autobus,
            ObjizdkovyJR = false,
            SeskupenieSpojov = false,
            PouzitieOznacnikov = false,
            PlatnostJROd = "01012024",
            PlatnostJRDo = "31122024",
            RozlisenieDopravcu = data.Dopravci[0].RozlisenieDopravcu,
            RozlisenieLinky = 1
        };
        
        data.Linky.Add(novaLinka);
        
        Console.WriteLine($"Nový počet liniek: {data.Linky.Count}");
        
        // Zapíš upravené dáta
        var writer = new JdfWriter("/cesta/k/vystupnemu/adresaru");
        writer.WriteAll(data);
        
        Console.WriteLine("Upravené JDF dáta boli zapísané.");
    }
    
    /// <summary>
    /// Príklad filtrovania dát
    /// </summary>
    public static void FilterDataExample()
    {
        Console.WriteLine("=== Príklad filtrovania JDF dát ===");
        
        var loader = new JdfLoader("/cesta/k/jdf/adresaru");
        var data = loader.LoadAll();
        
        // Filtruj len mestské linky
        var mestskeLinky = data.Linky
            .Where(l => l.Typ == TypLinky.Mestska)
            .ToList();
        
        Console.WriteLine($"Počet mestských liniek: {mestskeLinky.Count}");
        
        // Filtruj len tramvajové dopravné prostriedky
        var tramvajeLinky = data.Linky
            .Where(l => l.DopravnyProstriedok == DopravnyProstriedok.Elektricka)
            .ToList();
        
        Console.WriteLine($"Počet tramvajových liniek: {tramvajeLinky.Count}");
        
        // Filtruj zastávky v konkrétnom meste
        var zastávkyVBratislave = data.Zastavky
            .Where(z => z.NazovObce.Contains("Bratislava"))
            .ToList();
        
        Console.WriteLine($"Počet zastávok v Bratislave: {zastávkyVBratislave.Count}");
    }
    
    /// <summary>
    /// Príklad analýzy spojov na linke
    /// </summary>
    public static void AnalyzeConnectionsExample()
    {
        Console.WriteLine("=== Príklad analýzy spojov na linke ===");
        
        var loader = new JdfLoader("/cesta/k/jdf/adresaru");
        var data = loader.LoadAll();
        
        // Vyber linku
        var linka = data.Linky.FirstOrDefault();
        if (linka == null)
        {
            Console.WriteLine("Žiadne linky nenájdené.");
            return;
        }
        
        Console.WriteLine($"Analýza linky č. {linka.Cislo}: {linka.Nazov}");
        
        // Spočítaj spoje na linke
        var spojeNaLinke = data.Spoje
            .Where(s => s.CisloLinky == linka.Cislo && s.RozliseniLinky == linka.RozlisenieLinky)
            .ToList();
        
        Console.WriteLine($"Počet spojov: {spojeNaLinke.Count}");
        
        // Spočítaj zastávky na linke
        var zastavkyNaLinke = data.Zaslinky
            .Where(z => z.CisloLinky == linka.Cislo && z.RozlisenieLinky == linka.RozlisenieLinky)
            .OrderBy(z => z.CisloTarifni)
            .ToList();
        
        Console.WriteLine($"Počet zastávok: {zastavkyNaLinke.Count}");
        
        // Vypíš prvých 5 zastávok
        Console.WriteLine("\nPrvých 5 zastávok:");
        foreach (var zaslinka in zastavkyNaLinke.Take(5))
        {
            var zastavka = data.Zastavky.FirstOrDefault(z => z.Cislo == zaslinka.CisloZastavky);
            if (zastavka != null)
            {
                Console.WriteLine($"  {zaslinka.CisloTarifni}. {zastavka.NazovObce} - {zastavka.BlizkeMiesto}");
            }
        }
    }
}
