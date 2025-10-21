namespace ScottPlotTest
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Input

module UI = 
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open ScottPlot.Avalonia

    // -- Extend AvaPlot with an 'onLoaded' attribute to run code once the control is loaded --
    type AvaPlot with
        static member onLoaded<'t when 't :> AvaPlot>(handler: AvaPlot -> unit) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription<Avalonia.Interactivity.RoutedEventArgs>(
                Control.LoadedEvent,
                (fun args -> handler(args.Source :?> AvaPlot))
            )

    // -- Application model/messages (unused in this simple example) --
    type Model = unit
    type Msg = Test

    let init () = ()
    let update msg model = 
        match msg with 
        | Test -> model

    // -- The view: an AvaPlot that fills the window -- 
    //    When loaded, we add a scatter plot and refresh it.
    let view model dispatch =
        DockPanel.create [
            DockPanel.children [
                Button.create [
                    Button.content "test"
                    Button.onClick(fun x -> dispatch Test)
                    DockPanel.dock Dock.Top
                ]
                View.createGeneric<AvaPlot> [
                    // Run this once when the plot control is loaded
                    AvaPlot.onLoaded (fun (ap:AvaPlot) ->
                        // Add some data to the plot
                        let dataX = [| 1; 2; 3; 4; 5 |]
                        let dataY = [| 1; 4; 9; 16; 25 |]
                        ap.Plot.Add.Scatter(dataX, dataY) |> ignore
                        // Refresh to force a redraw
                        ap.Refresh()
                    )
                    // Make the plot stretch to fill the window
                    Control.horizontalAlignment HorizontalAlignment.Stretch
                    Control.verticalAlignment   VerticalAlignment.Stretch
                ]
            ]
        ]

module Host =
    open Elmish
    open Avalonia.FuncUI.Hosts
    open Avalonia.FuncUI.Elmish

    // -- Create the main window using FuncUI Elmish --
    type MainWindow() as this =
        inherit HostWindow()
        do
            base.Title  <- "ScottPlot + Avalonia.FuncUI Demo"
            base.Width  <- 600.0
            base.Height <- 400.0
            Program.mkSimple UI.init UI.update UI.view
            |> Program.withHost this
            |> Program.runWithAvaloniaSyncDispatch ()