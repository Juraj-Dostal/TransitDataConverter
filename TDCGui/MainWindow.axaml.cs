using Avalonia.Controls;
using TDCGui.ViewModels;

namespace TDCGui;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}