using System.ComponentModel;

namespace TDCLibrary.JdfModel.Enums;

public enum PevnyKodOznacenie
{
    [Description("X")]
    PracovneDni,
    
    [Description("+")]
    NedeleAveSviatky,
    
    [Description("1")]
    Pondelok,
    
    [Description("2")]
    Utorok,
    
    [Description("3")]
    Streda,
    
    [Description("4")]
    Stvrtok,
    
    [Description("5")]
    Piatok,
    
    [Description("6")]
    Sobota,
    
    [Description("7")]
    Nedela,
    
    [Description("R")]
    MiestenkaVolitelna,
    
    [Description("#")]
    MiestenkaPovinná,
    
    [Description("|")]
    PrejazdZastavkou,
    
    [Description("<")]
    AlternatívnaTrasa,
    
    [Description("@")]
    Bezbarierovost,
    
    [Description("%")]
    Obcerstvenie,
    
    [Description("W")]
    VerejneWCVZastavke,
    
    [Description("w")]
    VerejneWCVZastavkeBezbarierove,
    
    [Description("x")]
    ZastavkaNaZiadost,
    
    [Description("~")]
    PrestupNaMHD,
    
    [Description("I")]
    SystémIDS,
    
    [Description("(")]
    LenVystup,
    
    [Description(")")]
    LenVstup,
    
    [Description("$")]
    HranicniPrechod,
    
    [Description("{")]
    BezbarierovéSDopomocou,
    
    [Description("}")]
    TazkoZrakovoPostihnuti,
    
    [Description("[")]
    PrepravaBatazin,
    
    [Description("O")]
    PrepravaBicyklov,
    
    [Description("v")]
    PrestupNaVlak,
    
    [Description("s")]
    SamoobsluznySystem,
}