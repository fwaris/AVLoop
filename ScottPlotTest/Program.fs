namespace ScottPlotTest
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Input

type App() =
    inherit Avalonia.Application()

    override this.Initialize() =
        this.AttachDevTools(Diagnostics.DevToolsOptions(Gesture=KeyGesture(Key.F12)))

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let win = Host.MainWindow()
            desktopLifetime.MainWindow <- win
        | _ -> ()


module Pgm = 
    open Avalonia

    [<EntryPoint>]
    let main args =
        // Build and run the Avalonia application
        AppBuilder
            .Configure<App>()    // your Avalonia Application type
            .UsePlatformDetect()
            //.UseSkia()                   // required for ScottPlot rendering
            .StartWithClassicDesktopLifetime(args)
