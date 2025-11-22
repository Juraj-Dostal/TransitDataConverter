using TDCLibrary.JdfModel;

namespace TDCLibrary;

/// <summary>
/// Kontajner pre všetky JDF dáta načítané zo súborov.
/// Obsahuje všetky dostupné údaje z JDF datasetu.
/// </summary>
public class JdfData
{
    /// <summary>
    /// Verzia JDF (VerzeJDF.txt - POVINNÝ)
    /// </summary>
    public VerzeJDF? VerzeJDF { get; set; }
    
    /// <summary>
    /// Zoznam dopravných spoločností (Dopravci.txt - POVINNÝ)
    /// </summary>
    public List<Dopravci> Dopravci { get; set; } = new();
    
    /// <summary>
    /// Zoznam zastávok (Zastavky.txt - POVINNÝ)
    /// </summary>
    public List<Zastavky> Zastavky { get; set; } = new();
    
    /// <summary>
    /// Zoznam liniek (Linky.txt - POVINNÝ)
    /// </summary>
    public List<Linky> Linky { get; set; } = new();
    
    /// <summary>
    /// Zoznam zastávok liniek (Zaslinky.txt - POVINNÝ)
    /// </summary>
    public List<Zaslinky> Zaslinky { get; set; } = new();
    
    /// <summary>
    /// Zoznam spojov (Spoje.txt - POVINNÝ)
    /// </summary>
    public List<Spoje> Spoje { get; set; } = new();
    
    /// <summary>
    /// Zoznam zastávok spojov (Zasspoje.txt - POVINNÝ)
    /// </summary>
    public List<Zasspoje> Zasspoje { get; set; } = new();
    
    /// <summary>
    /// Zoznam časových kódov (Caskody.txt - POVINNÝ)
    /// </summary>
    public List<Caskody> Caskody { get; set; } = new();
    
    /// <summary>
    /// Zoznam pevných kódov (Pevnykod.txt - POVINNÝ)
    /// </summary>
    public List<Pevnykod> PevnyKod { get; set; } = new();
    
    /// <summary>
    /// Zoznam označníkov (Oznacniky.txt - VOLITEĽNÝ)
    /// </summary>
    public List<Oznacniky> Oznacniky { get; set; } = new();
}
