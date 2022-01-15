# AVLoop
A little bit of infractructure required to support Avalonia windows in F# interactive, 
courtesy of the discussion in [this issue](https://github.com/fsprojects/Avalonia.FuncUI/issues/147).

Run the following script code before creating Avalonia windows.

```F#
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
```

Run the above snippent interactively first. Wait till you see a message "avalonia initialized" and then proceed to submit other code.

If you run this script along with window creation code in the same submit then that will not work. 
It seems the F# interactive main loop thread needs to be free to 'Run' the event loop (which actually initializes Avalonia).

### Window example
Here is an example of a simple window
```F#
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

```