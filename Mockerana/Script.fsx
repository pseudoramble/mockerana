#r "../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#load "Mockerana.fs"
#load "JsonProcessor.fs"

open Mockerana
 
let record = Record [
 ("name", String);
 ("total", Money);
 ("location", Record [
   ("address", String)
   ("state", String)
   ("zip", Constrained (String, [Max 5; Min 5]))
 ]);
 ("steps", Array(
   Record [
     ("amount", Money)
     ("processed", Boolean)
   ]
 ))
]

let rng = new System.Random()

printfn "Result \n---------------\n%s" <| Mockerana.JsonProcessor.run record