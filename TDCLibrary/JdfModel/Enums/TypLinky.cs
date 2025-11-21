using System.ComponentModel;

namespace TDCLibrary.JdfModel.Enums;

public enum TypLinky
{
    // A Městská
    // B Městská s obsluhou příměstských oblastí
    // N Mezinárodní – s vyloučenou vnitrostátní dopravou
    // P Mezinárodní – s povolenou vnitrostátní dopravou
    // V Vnitrostátní – vnitrokrajská
    // Z Vnitrostátní – mezikrajská
    // D Vnitrostátní – dálková
    
    [Description ("A")]
    Mestska,
    [Description ("B")]
    MestskaSObsluhouPrimestskychOblasti,
    [Description ("N")]
    MezinarodniSVyloucenouVnitrastatniDopravou,
    [Description ("P")]
    MezinarodniSPovolenaVnitrastatniDopravou,
    [Description ("V")]
    VnitrastatniVnitrokrajska,
    [Description ("Z")]
    VnitrastatniMezikrajska,
    [Description ("D")]
    VnitrastatniDalkova
}