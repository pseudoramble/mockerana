#r "../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#load "Mockerana.fs"
#load "JsonProcessor.fs"

open Mockerana
 
let record = Record [
 ("name", String);
 ("total", Number);
 ("location", Record [
   ("address", String)
   ("state", String)
   ("zip", Constrained (String, [Max 5; Min 5]))
 ]);
 ("steps", Array [
   Record [
     ("amount", Real)
     ("processed", Boolean)
   ]
 ])
]

Mockerana.JsonProcessor.run record