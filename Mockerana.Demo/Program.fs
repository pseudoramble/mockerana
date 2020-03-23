module Mockerana.Demo

open System
open Mockerana

[<EntryPoint>]
let main argv =
  printfn "Simple single string\n------------"
  printfn "%s\n" (String |> JsonProcessor.toString)

  printfn "Simple single number\n------------"
  printfn "%s\n" (Number |> JsonProcessor.toString)

  printfn "Simple single integer\n------------"
  printfn "%s\n" (Integer |> JsonProcessor.toString)

  printfn "List of strings\n------------"
  printfn "%s\n" (Array (String) |> JsonProcessor.toString)

  printfn "List of numbers\n------------"
  printfn "%s\n" (Array (Number) |> JsonProcessor.toString)

  printfn "Names\n------------"
  let maleName = Name (Some Male) |> JsonProcessor.toString
  let femaleName = Name (Some Female) |> JsonProcessor.toString
  printfn "Female: %s, Male: %s\n" femaleName maleName

  printfn "Dates\n------------"
  let sampleDateTime = DateTime.Parse("2018-01-01")
  let noBoundsDate = DateTime (None, None) |> JsonProcessor.toString
  let earliest = DateTime (Some sampleDateTime, None) |> JsonProcessor.toString
  let latest = DateTime (None, Some sampleDateTime) |> JsonProcessor.toString
  let between = DateTime (Some sampleDateTime, Some DateTime.UtcNow) |> JsonProcessor.toString
  printfn "No Bounds: %s\nEaliest: %s\nLatest: %s\nBetween: %s\n" noBoundsDate earliest latest between

  printfn "Money\n------------"
  let thousands = Money(Some Thousands) |> JsonProcessor.toString
  let hundreds = Money(Some Hundreds) |> JsonProcessor.toString
  let tens = Money(Some Tens) |> JsonProcessor.toString
  printfn "$%s, $%s, $%s\n" thousands hundreds tens

  printfn "Location (Address in the USA)\n------------"
  printfn "%s\n" (Location |> JsonProcessor.toString)

  printfn "Choose one from [\"a\", \"b\", \"c\"]\n------------"
  let oneOfStr = OneOf [Primitive.String "a"; Primitive.String "b"; Primitive.String "c"]
  printfn "%s\n" (oneOfStr |> JsonProcessor.toString)

  printfn "Choose one from [1, 2, 3]\n------------"
  let oneOfInt = OneOf [Primitive.Integer 1; Primitive.Integer 2; Primitive.Integer 3]
  printfn "%s\n" (oneOfInt |> JsonProcessor.toString)

  printfn "Specific literal values\n------------"
  let exactlyStr = Exactly <| Primitive.String "hello"
  let exactlyNumber = Exactly <| Primitive.Number 3.14159m
  printfn "String = %s, Number = %s\n" (exactlyStr |> JsonProcessor.toString) (exactlyNumber |> JsonProcessor.toString)

  printfn "Complex record/object\n------------"
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
  printfn "%s\n" <| JsonProcessor.toString record
    
  0