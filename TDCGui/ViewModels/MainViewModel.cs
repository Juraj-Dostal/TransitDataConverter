using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using ReactiveUI;
using TDCLibrary;
using TDCLibrary.GtsfModel;
using TDCGui.Views.GtfsViews;

namespace TDCGui.ViewModels;

public class MainViewModel : ReactiveObject
{
    private string? _selectedFolder;
    private string? _convertFolder;
    private string _statusMessage = "Pripravený";
    private GtfsData _data = new();
    private DataTypeItem? _selectedDataType;
    private UserControl? _currentView;

    public string? SelectedFolder
    {
        get => _selectedFolder;
        set => this.RaiseAndSetIfChanged(ref _selectedFolder, value);
    }

    public string? ConvertFolder
    {
        get => _convertFolder;
        set => this.RaiseAndSetIfChanged(ref _convertFolder, value);
    }
    
    public string StatusMessage
    {
        get => _statusMessage;
        set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
    }

    public GtfsData Data
    {
        get => _data;
        set
        {
            this.RaiseAndSetIfChanged(ref _data, value);
            UpdateCurrentView(); // Refresh view with new data
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
            "Agency" => new AgencyDataControl
            {
                DataContext = new ObservableCollection<Agency>(Data.Agencies)
            },
            "Routes" => new RoutesView
            {
                DataContext = new ObservableCollection<Route>(Data.Routes)
            },
            "Stops" => new StopsView
            {
                DataContext = new ObservableCollection<Stop>(Data.Stops)
            },
            "Trips" => new TripsView
            {
                DataContext = new ObservableCollection<Trip>(Data.Trips)
            },
            "StopTimes" => new StopTimesView
            {
                DataContext = new ObservableCollection<StopTime>(Data.StopTimes)
            },
            "Calendar" => new CalendarView
            {
                DataContext = new ObservableCollection<TDCLibrary.GtsfModel.Calendar>(Data.Calendars)
            },
            "CalendarDates" => new CalendarDatesView
            {
                DataContext = new ObservableCollection<CalendarDate>(Data.CalendarDates)
            },
            "FareAttributes" => new FareAttributesView
            {
                DataContext = new ObservableCollection<FareAttribute>(Data.FareAttributes)
            },
            "FareRules" => new FareRulesView
            {
                DataContext = new ObservableCollection<FareRule>(Data.FareRules)
            },
            "Shapes" => new ShapesView
            {
                DataContext = new ObservableCollection<Shape>(Data.Shapes)
            },
            "Frequencies" => new FrequenciesView
            {
                DataContext = new ObservableCollection<Frequency>(Data.Frequencies)
            },
            "Transfers" => new TransfersView
            {
                DataContext = new ObservableCollection<Transfer>(Data.Transfers)
            },
            "Pathways" => new PathwaysView
            {
                DataContext = new ObservableCollection<Pathway>(Data.Pathways)
            },
            "Levels" => new LevelsView
            {
                DataContext = new ObservableCollection<Level>(Data.Levels)
            },
            "Translations" => new TranslationsView
            {
                DataContext = new ObservableCollection<Translation>(Data.Translations)
            },
            "Attributions" => new AttributionsView
            {
                DataContext = new ObservableCollection<Attribution>(Data.Attributions)
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
        new DataTypeItem { DisplayName = "Agency", DataType = "Agency" },
        new DataTypeItem { DisplayName = "Routes", DataType = "Routes" },
        new DataTypeItem { DisplayName = "Stops", DataType = "Stops" },
        new DataTypeItem { DisplayName = "Trips", DataType = "Trips" },
        new DataTypeItem { DisplayName = "Stop Times", DataType = "StopTimes" },
        new DataTypeItem { DisplayName = "Calendar", DataType = "Calendar" },
        new DataTypeItem { DisplayName = "Calendar Dates", DataType = "CalendarDates" },
        new DataTypeItem { DisplayName = "Fare Attributes", DataType = "FareAttributes" },
        new DataTypeItem { DisplayName = "Fare Rules", DataType = "FareRules" },
        new DataTypeItem { DisplayName = "Shapes", DataType = "Shapes" },
        new DataTypeItem { DisplayName = "Frequencies", DataType = "Frequencies" },
        new DataTypeItem { DisplayName = "Transfers", DataType = "Transfers" },
        new DataTypeItem { DisplayName = "Pathways", DataType = "Pathways" },
        new DataTypeItem { DisplayName = "Levels", DataType = "Levels" },
        new DataTypeItem { DisplayName = "Translations", DataType = "Translations" },
        new DataTypeItem { DisplayName = "Attributions", DataType = "Attributions" }
    };

    public ReactiveCommand<Unit, Unit> SelectFolderCommand { get; }
    public ReactiveCommand<Unit, Unit> LoadDataCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveDataCommand { get; }
    public ReactiveCommand<Unit, Unit> SelectConvertFolderCommand { get; }

    public MainViewModel()
    {
        var ui = RxApp.MainThreadScheduler;
        
        // Simple commands without reactive CanExecute to avoid cross-thread issues
        SelectFolderCommand = ReactiveCommand.CreateFromTask(SelectFolderAsync, outputScheduler: ui);
        SelectConvertFolderCommand = ReactiveCommand.CreateFromTask(SelectConvertFolderAsync, outputScheduler: ui);
        LoadDataCommand = ReactiveCommand.CreateFromTask(LoadDataAsync, outputScheduler: ui);
        SaveDataCommand = ReactiveCommand.CreateFromTask(SaveDataAsync, outputScheduler: ui);
        
        // Set default selection to Agency
        SelectedDataType = DataTypeItems[0];
    }

    private async Task SelectFolderAsync()
    {
        var window = GetWindow();
        if (window == null) return;

        var folders = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Vyber GTFS priečinok",
            AllowMultiple = false
        });

        if (folders.Count > 0)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                SelectedFolder = folders[0].Path.LocalPath;
            });
        }
    }

    private async Task SelectConvertFolderAsync()
    {
        var window = GetWindow();
        if (window == null) return;

        var folders = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Vyber cieľový priečinok pre JDF",
            AllowMultiple = false
        });

        if (folders.Count > 0)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                ConvertFolder = folders[0].Path.LocalPath;
            });
        }
    }

    private async Task LoadDataAsync()
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
                StatusMessage = "Načítavam GTFS dáta...";
            });

            var loader = new GtfsLoader(SelectedFolder);
            var loadedData = await Task.Run(() => loader.LoadAll());

            Console.WriteLine($"DEBUG: Loaded {loadedData.Agencies.Count} agencies, {loadedData.Routes.Count} routes, {loadedData.Stops.Count} stops");

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Data = loadedData;
                
                // Force refresh of CurrentView
                var currentSelection = SelectedDataType;
                SelectedDataType = null; // Reset
                SelectedDataType = currentSelection; // Re-apply to trigger UpdateCurrentView

                Console.WriteLine($"DEBUG: Data assigned. Current view type: {SelectedDataType?.DataType}");
                StatusMessage = $"Načítané:{Data.Routes.Count} routes, ";
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DEBUG ERROR: {ex}");
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                StatusMessage = $"Chyba pri načítaní: {ex.Message}";
            });
        }
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
                StatusMessage = "Ukladám GTFS dáta...";
            });

            await Task.Run(() =>
            {
                var writer = new GtfsWriter(SelectedFolder);
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

    private async Task ConvertToJdfAsync()
    {
        if (string.IsNullOrWhiteSpace(ConvertFolder))
        {
            StatusMessage = "Chyba: Nie je vybraný cieľový priečinok pre JDF!";
            return;
        }

        if (Data.Agencies.Count == 0 && Data.Routes.Count == 0)
        {
            StatusMessage = "Chyba: Najprv načítajte GTFS dáta!";
            return;
        }

        try
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                StatusMessage = "Konvertujem GTFS do JDF...";
            });

            JdfData jdfData = null;
            await Task.Run(() =>
            {
                jdfData = Gtfs2Jdf.Convert(Data);
                var writer = new JdfWriter(ConvertFolder);
                writer.WriteAll(jdfData);
            });

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                StatusMessage = "Konverzia dokončená!";
                
                // Open JDF window with converted data
                var jdfWindow = new JdfWindow();
                var viewModel = new JdfViewModel();
                viewModel.LoadJdfData(jdfData, ConvertFolder);
                jdfWindow.DataContext = viewModel;
                jdfWindow.Show();
            });
        }
        catch (Exception ex)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                StatusMessage = $"Chyba pri konverzii: {ex.Message}";
            });
        }
    }

    private static Window? GetWindow()
        => Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime d ? d.MainWindow : null;
    
    
}