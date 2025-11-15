using System.Collections.Generic;
using ReactiveUI;

namespace TDCGui.ViewModels;

public class DataTypeItem : ReactiveObject
{
    public string DisplayName { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
}
