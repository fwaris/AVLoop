#load @"eventloop.fsx" // run this first separately
open System
open Avalonia.Controls

type DataItem = {A:string; B:int; C:DateTime}

type W<'t>(data:'t seq) as this =
    inherit Window()
    let dg = DataGrid()
    do
        this.Height <- 600.0
        this.Width <- 800.0
        dg.AutoGenerateColumns <- true
        dg.ItemsSource <- data
        this.Content <- dg

let inline showData data =
    let w = W(data)
    w.Show()

let ds = [for i in 1..1000 -> {A=string i; B=i; C=DateTime.Now.AddMinutes(i)}]

showData ds



