#I "/home/dswain/.nuget/packages"
#r "fsharp.data/3.0.0/lib/netstandard2.0/FSharp.Data.dll"
#r "fsharp.data/3.0.0/lib/netstandard2.0/FSharp.Data.DesignTime.dll"
#load "Mockerana.fs"
#load "DataLoader.fs"
#load "FormatProcessor.fs"
#load "JsonProcessor.fs"

open Mockerana

let record = Record [
 ("name", Name None)
 ("expires", DateTime (Some System.DateTime.Now, Some (System.DateTime(2019, 09, 20))))
 ("total", Money(Some Hundreds))
 ("location", Location)
 ("id", Mockerana.Supports.Format.create "warehouse:{%int:min=1000;max=9999%},item:{%str:max=5%}")
 ("steps", Array(
   Record [
     ("amount", Money(Some Tens))
     ("processed", Boolean)
   ]
 ))
 ("status", OneOf [Primitive.String "Open"; Primitive.String "Closed"])
]

let rng = System.Random()

printfn "Result \n---------------\n%s" <| Mockerana.JsonProcessor.run record
