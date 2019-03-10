module Mockerana.Demo

open System
open Mockerana

[<EntryPoint>]
let main argv =
  let record = Record [
   ("name", Name None)
   ("expires", DateTime (Some DateTime.Now, Some (System.DateTime(2019, 09, 20))))
   ("total", Money(Some Hundreds))
   ("location", Location)
   ("id", Supports.Format.create "warehouse:{%int:min=1000;max=9999%},item:{%str:max=5%}")
   ("steps", Array(
     Record [
       ("amount", Money(Some Tens))
       ("processed", Boolean)
     ]
   ))
   ("status", OneOf [Primitive.String "Open"; Primitive.String "Closed"])
  ]

  let rng = Random()

  printfn "Result \n---------------\n%s" <| JsonProcessor.run record
    
  0