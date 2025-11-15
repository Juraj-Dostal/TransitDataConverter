using Avalonia;
using System;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace TDCGui;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // Ensure ReactiveUI uses Avalonia UI thread
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }
    // public static void Main(string[] args) => BuildAvaloniaApp()
    //     .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI(); // wires Avalonia + ReactiveUI schedulers

}