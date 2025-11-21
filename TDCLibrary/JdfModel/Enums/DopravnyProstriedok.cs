using System.ComponentModel;

namespace TDCLibrary.JdfModel.Enums;

public enum DopravnyProstriedok
{
    [Description ("A")]
    Autobus,
    [Description ("E")]
    Tramvaj,
    [Description ("L")]
    LanovaDraha,
    [Description ("M")]
    Metro,
    [Description ("P")]
    Privoz,
    [Description ("T")]
    Trolejbus
}