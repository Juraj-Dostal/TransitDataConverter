using System.ComponentModel;

namespace TDCLibrary.JdfModel.Enums;

public enum PevnyKodOznacenie
{
    [Description("R")]
    MiestenkaVolitelna,
    
    [Description("X")]
    PracovneDni,
    
    [Description("a")]
    NedeleAveSviatky,
    
    [Description("@")]
    Bezbarierovost,
    
    [Description("7")]
    Nedela,
    
    [Description("6")]
    Sobota,
    
    [Description("x")]
    ZastavkaNaZiadost,
    
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
    
    
    
    [Description("#")]
    MiestenkaPovinná,
    
    [Description("|")]
    PrejazdZastavkou,
    
    [Description("<")]
    AlternatívnaTrasa,
    
    [Description("%")]
    Obcerstvenie,
    
    [Description("W")]
    VerejneWCVZastavke,
    
    [Description("w")]
    VerejneWCVZastavkeBezbarierove,
    
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