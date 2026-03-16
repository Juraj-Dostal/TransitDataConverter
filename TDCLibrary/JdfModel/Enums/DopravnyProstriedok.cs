using System.ComponentModel;

namespace TDCLibrary.JdfModel.Enums;

public enum DopravnyProstriedok
{
    [Description ("T")]
    Autobus,
    [Description ("E")]
    Elektricka,
    [Description ("L")]
    LanovaDraha,
    [Description ("M")]
    Metro,
    [Description ("P")]
    Trajekt,
    [Description ("O")]
    Trolejbus
}