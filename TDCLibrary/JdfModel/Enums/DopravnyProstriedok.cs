using System.ComponentModel;

namespace TDCLibrary.JdfModel.Enums;

public enum DopravnyProstriedok
{
    [Description ("A")]
    Autobus,
    [Description ("E")]
    Elektricka,
    [Description ("L")]
    LanovaDraha,
    [Description ("M")]
    Metro,
    [Description ("P")]
    Trajekt,
    [Description ("T")]
    Trolejbus
}