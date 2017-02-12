#r "../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#load "Mockerana.fs"
#load "Constraints.fs"
#load "DataLoader.fs"
#load "JsonProcessor.fs"

open Mockerana
 
let record = Record [
 ("name", Name None);
 ("total", Money(Some Hundreds))
 ("location", Location);
 ("steps", Array(
   Record [
     ("amount", Money(Some Tens))
     ("processed", Boolean)
   ]
 ))
]

let rng = new System.Random()

printfn "Result \n---------------\n%s" <| Mockerana.JsonProcessor.run record