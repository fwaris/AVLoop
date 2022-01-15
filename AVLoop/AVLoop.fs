module AVLoop
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Threading
open Avalonia.Markup.Xaml.Styling
open System
open System.Threading

type Theme = Fluent | Default
type Mode = Light | Dark

let mutable private theme = (Default,Dark)

let private appCreated = ref false
let reset() = appCreated.Value <- false

let accentsDark() : Styling.IStyle seq =
    [
        StyleInclude(baseUri=null, Source=Uri("avares://Avalonia.Themes.Fluent/Accents/AccentColors.xaml"))
        StyleInclude(baseUri=null, Source=Uri("avares://Avalonia.Themes.Fluent/Accents/Base.xaml"))
        StyleInclude(baseUri=null, Source=Uri("avares://Avalonia.Themes.Fluent/Accents/BaseDark.xaml"))
        StyleInclude(baseUri=null, Source=Uri("avares://Avalonia.Themes.Default/Accents/BaseDark.xaml"))
        StyleInclude(baseUri=null, Source=Uri("avares://Avalonia.Themes.Default/DefaultTheme.xaml"))
    ]

let accentsLight() : Styling.IStyle seq =
    [
        StyleInclude(baseUri=null, Source=Uri("avares://Avalonia.Themes.Fluent/Accents/AccentColors.xaml"))
        StyleInclude(baseUri=null, Source=Uri("avares://Avalonia.Themes.Fluent/Accents/Base.xaml"))
        StyleInclude(baseUri=null, Source=Uri("avares://Avalonia.Themes.Fluent/Accents/BaseLight.xaml"))
        StyleInclude(baseUri=null, Source=Uri("avares://Avalonia.Themes.Default/Accents/BaseLight.xaml"))
        StyleInclude(baseUri=null, Source=Uri("avares://Avalonia.Themes.Default/DefaultTheme.xaml"))
    ]
    
let loadFluent (app:Application) mode =
    let theme = new Themes.Fluent.FluentTheme(baseUri=null)
    theme.Mode <- match mode with Dark -> Themes.Fluent.FluentThemeMode.Dark | Light -> Themes.Fluent.FluentThemeMode.Light
    let dgTheme = StyleInclude(baseUri=null, Source=Uri("avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml"))        
    app.Styles.AddRange (match mode with Dark -> accentsDark() | Light -> accentsLight())
    app.Styles.Add(dgTheme)
    app.Styles.Add(theme)

let loadDefault (app:Application) mode =
    let theme = new Themes.Default.DefaultTheme()  
    let dgTheme = StyleInclude(baseUri=null, Source=Uri("avares://Avalonia.Controls.DataGrid/Themes/Default.xaml"))        
    app.Styles.AddRange (match mode with Dark -> accentsDark() | Light -> accentsLight())
    app.Styles.Add(dgTheme)
    app.Styles.Add(theme)

type App() =
    inherit Application()
    override this.Initialize() =
        ///loadFluent this FluentThemeMode.Dark
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
    Dispatcher.UIThread.InvokeAsync(f) 
    |> Async.AwaitTask 
    |> Async.RunSynchronously 

