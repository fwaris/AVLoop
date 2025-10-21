
#r @"nuget: AVLoop, 1.6.4"
#r "nuget: Avalonia.FuncUI.Elmish, 1.5.1"
#r "nuget: ScottPlot.Avalonia, 5.1.57"

open FSharp.Compiler.Interactive
open AVLoop

let install(theme) =
        fsi.EventLoop <- {new IEventLoop with 
                                member x.Run() = 
                                    createApp(theme, [||])
                                    false //dummy
                                member x.Invoke(f) = disp f
                                member x.ScheduleRestart() = () //dummy
                        }

install(Default,Dark) //wait till initialization message before submitting more cod

#load "../View.fs"

let w = ScottPlotTest.Host.MainWindow()
w.Show()



