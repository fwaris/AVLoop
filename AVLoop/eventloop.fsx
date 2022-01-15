
//#i @"nuget: C:\Users\fwaris\Source\Repos\AVLoop\AVLoop\bin\Debug"
#r "nuget: AVLoop"

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

install(Default,Dark) //wait till initialization message before submitting more code

(*
//Test. 
open Avalonia
let win1 = Controls.Window()
win1.Width <- 300
win1.Height <- 300
let btn1 = Controls.Button()
btn1.Width <- 100
btn1.Height <- 50
btn1.Content <- "Click me"
btn1.Click |> Observable.subscribe (fun _ -> btn1.Content <- "Clicked!")
win1.Content <- btn1
win1.Show()

*)
