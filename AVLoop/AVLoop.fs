module AVLoop
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Threading
open Avalonia.Styling
open Avalonia.Markup.Xaml.Styling
open System

type Theme = Fluent | Default
type Mode = Light | Dark

let mutable private theme = (Default,Dark)

let private appCreated = ref false
let reset() = appCreated.Value <- false

type Styles with
    member this.Load (source: string) = 
        let style = StyleInclude(baseUri = null)
        style.Source <- Uri(source)
        this.Add(style)
    
let loadFluent (app:Application) mode =
    let theme = new Themes.Fluent.FluentTheme()
    app.RequestedThemeVariant <- match mode with Dark -> ThemeVariant.Dark | _ -> ThemeVariant.Light
    app.Styles.Add(theme)
    app.Styles.Load "avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml"

let loadDefault (app:Application) mode =
    let theme = new Themes.Simple.SimpleTheme()
    app.RequestedThemeVariant <- match mode with Dark -> ThemeVariant.Dark | _ -> ThemeVariant.Light
    app.Styles.Add(theme)
    app.Styles.Load "avares://Avalonia.Controls.DataGrid/Themes/Simple.xaml"


type App() =
    inherit Application()
    override this.Initialize() =        
        match theme with
        | Default, Dark  -> loadDefault this Dark
        | Default, Light -> loadDefault this Light
        | Fluent, Dark   -> loadFluent this Dark
        | Fluent, Light  -> loadFluent this Light
    
    override x.OnFrameworkInitializationCompleted() =
        match x.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->            
            desktopLifetime.ShutdownMode <- Controls.ShutdownMode.OnExplicitShutdown
            printfn "avalonia initialized"
        | _ -> ()

let createApp(inputTheme, args) = 
    if not appCreated.Value then        
        printfn "initializing avalonia ..."    
        appCreated.Value <- true
        theme <- inputTheme
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args) |> ignore
    else
        printfn "avalonia already initialized or an error occurred in the previous attempt"

let disp (f:unit -> 'a) = 
    Dispatcher.UIThread.InvokeAsync(f).GetTask()
    |> Async.AwaitTask 
    |> Async.RunSynchronously 

