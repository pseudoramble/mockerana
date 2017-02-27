#r "../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#load "Mockerana.fs"
#load "Constraints.fs"
#load "DataLoader.fs"
#load "JsonProcessor.fs"

open Mockerana


let record = Record [
 ("name", Name None)
 ("expires", DateTime (Some System.DateTime.Now, Some (new System.DateTime(2019, 09, 20))))
 ("total", Money(Some Hundreds))
 ("location", Location)
 ("steps", Array(
   Record [
     ("amount", Money(Some Tens))
     ("processed", Boolean)
   ]
 ))
 ("status", OneOf [Primitive.String "Open"; Primitive.String "Closed"])
]

let rng = new System.Random()

printfn "Result \n---------------\n%s" <| Mockerana.JsonProcessor.run record