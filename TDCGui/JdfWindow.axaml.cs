using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TDCGui.ViewModels;

namespace TDCGui;

public partial class JdfWindow : Window
{
    public JdfWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
