module AVLoop
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Threading
open Avalonia.Markup.Xaml.Styling
open System
open System.Threading

let private appCreated = ref false

type App() =
    inherit Application()
    override this.Initialize() =
        this.Styles.AddRange [ 
            new StyleInclude(baseUri=null, Source = Uri("resm:Avalonia.Themes.Default.DefaultTheme.xaml?assembly=Avalonia.Themes.Default"))
            new StyleInclude(baseUri=null, Source = Uri("resm:Avalonia.Themes.Default.Accents.BaseDark.xaml?assembly=Avalonia.Themes.Default"))
        ]
    override x.OnFrameworkInitializationCompleted() =
        match x.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->            
            desktopLifetime.ShutdownMode <- Controls.ShutdownMode.OnExplicitShutdown
            appCreated.Value <- true
            printfn "avalonia initialized"
        | _ -> ()

let createApp(args) = 
    if not appCreated.Value then        
        printfn "initializing avalonia ..."    
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args) |> ignore
    else
        printfn "avalonia already initialized"

let disp (f:unit -> 'a) = 
    Dispatcher.UIThread.InvokeAsync(f) 
    |> Async.AwaitTask 
    |> Async.RunSynchronously 

