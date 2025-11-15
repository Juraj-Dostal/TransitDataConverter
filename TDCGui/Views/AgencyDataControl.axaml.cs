using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;

namespace TDCGui.Views;

public partial class AgencyDataControl : UserControl
{
    public AgencyDataControl()
    {
        InitializeComponent();
        
        DataContextChanged += (_, __) =>
        {
            Debug.WriteLine($"DEBUG AgencyDataControl: DataContext changed to {DataContext?.GetType().Name ?? "null"}");
            if (DataContext is IEnumerable items)
            {
                var count = items is ICollection coll ? coll.Count : items.Cast<object>().Count();
                Debug.WriteLine($"DEBUG AgencyDataControl: Collection count = {count}");
            }
        };

        AttachedToVisualTree += (_, __) =>
        {
            Debug.WriteLine("DEBUG AgencyDataControl: Attached to visual tree");
        };
    }
}