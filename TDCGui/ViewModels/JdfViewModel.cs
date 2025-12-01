using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;
using TDCLibrary;
using TDCLibrary.JdfModel;
using TDCGui.Views.JdfViews;

namespace TDCGui.ViewModels;

public class JdfViewModel : ReactiveObject
{
    private string? _selectedFolder;
    private string _statusMessage = "Pripravený";
    private JdfData _data = new();
    private DataTypeItem? _selectedDataType;
    private UserControl? _currentView;

    public string? SelectedFolder
    {
        get => _selectedFolder;
        set => this.RaiseAndSetIfChanged(ref _selectedFolder, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
    }

    public JdfData Data
    {
        get => _data;
        set
        {
            this.RaiseAndSetIfChanged(ref _data, value);
            UpdateCurrentView();
        }
    }

    public DataTypeItem? SelectedDataType
    {
        get => _selectedDataType;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedDataType, value);
            UpdateCurrentView();
        }
    }

    public UserControl? CurrentView
    {
        get => _currentView;
        private set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }

    private void UpdateCurrentView()
    {
        if (SelectedDataType == null)
        {
            CurrentView = null;
            return;
        }

        // Create new control instance with current data
        CurrentView = SelectedDataType.DataType switch
        {
            "VerzeJDF" => new JdfVerzeView
            {
                DataContext = Data.VerzeJDF
            },
            "Dopravci" => new JdfDopravciView
            {
                DataContext = new ObservableCollection<Dopravci>(Data.Dopravci)
            },
            "Zastavky" => new JdfZastavkyView
            {
                DataContext = new ObservableCollection<Zastavky>(Data.Zastavky)
            },
            "Linky" => new JdfLinkyView
            {
                DataContext = new ObservableCollection<Linky>(Data.Linky)
            },
            "Zaslinky" => new JdfZaslinkyView
            {
                DataContext = new ObservableCollection<Zaslinky>(Data.Zaslinky)
            },
            "Spoje" => new JdfSpojeView
            {
                DataContext = new ObservableCollection<Spoje>(Data.Spoje)
            },
            "Zasspoje" => new JdfZasspojeView
            {
                DataContext = new ObservableCollection<Zasspoje>(Data.Zasspoje)
            },
            "Caskody" => new JdfCaskodyView
            {
                DataContext = new ObservableCollection<Caskody>(Data.Caskody)
            },
            "PevnyKod" => new JdfPevnykodView
            {
                DataContext = new ObservableCollection<Pevnykod>(Data.PevnyKod)
            },
            "Oznacniky" => new JdfOznacnikyView
            {
                DataContext = new ObservableCollection<Oznacniky>(Data.Oznacniky)
            },
            _ => CreateEmptyView()
        };
    }

    private UserControl CreateEmptyView()
    {
        return new UserControl
        {
            Content = new TextBlock
            {
                Text = $"Údaje pre {SelectedDataType?.DisplayName} - zatiaľ neimplementované",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                FontSize = 16
            }
        };
    }

    public List<DataTypeItem> DataTypeItems { get; } = new()
    {
        new DataTypeItem { DisplayName = "Verzia JDF", DataType = "VerzeJDF" },
        new DataTypeItem { DisplayName = "Dopravci", DataType = "Dopravci" },
        new DataTypeItem { DisplayName = "Zastávky", DataType = "Zastavky" },
        new DataTypeItem { DisplayName = "Linky", DataType = "Linky" },
        new DataTypeItem { DisplayName = "Zastávky liniek", DataType = "Zaslinky" },
        new DataTypeItem { DisplayName = "Spoje", DataType = "Spoje" },
        new DataTypeItem { DisplayName = "Zastávky spojov", DataType = "Zasspoje" },
        new DataTypeItem { DisplayName = "Časové kódy", DataType = "Caskody" },
        new DataTypeItem { DisplayName = "Pevný kód", DataType = "PevnyKod" },
        new DataTypeItem { DisplayName = "Označníky", DataType = "Oznacniky" }
    };

    public ReactiveCommand<Unit, Unit> SaveDataCommand { get; }

    public JdfViewModel()
    {
        var ui = RxApp.MainThreadScheduler;
        
        SaveDataCommand = ReactiveCommand.CreateFromTask(SaveDataAsync, outputScheduler: ui);
        
        // Set default selection
        SelectedDataType = DataTypeItems[0];
    }

    private async Task SaveDataAsync()
    {
        if (string.IsNullOrWhiteSpace(SelectedFolder))
        {
            StatusMessage = "Chyba: Nie je vybraný priečinok!";
            return;
        }

        try
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                StatusMessage = "Ukladám JDF dáta...";
            });

            await Task.Run(() =>
            {
                var writer = new JdfWriter(SelectedFolder);
                writer.WriteAll(Data);
            });

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                StatusMessage = "Uložené úspešne!";
            });
        }
        catch (Exception ex)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                StatusMessage = $"Chyba pri ukladaní: {ex.Message}";
            });
        }
    }

    public void LoadJdfData(JdfData data, string folderPath)
    {
        SelectedFolder = folderPath;
        Data = data;
        StatusMessage = $"Načítané JDF dáta z: {folderPath}";
    }
}
