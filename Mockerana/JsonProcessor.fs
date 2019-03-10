module JsonProcessor

open FSharp.Data
open Mockerana

let rng = System.Random()

let location () =
  let chosenLoc = DataLoader.Location.generate ()
  [
    ("address", JsonValue.String chosenLoc.address)
    ("city", JsonValue.String chosenLoc.city)
    ("state", JsonValue.String chosenLoc.state)
    ("zip", JsonValue.String chosenLoc.zip)
  ]

let makeFullName sex = 
  match sex with 
  | Gender.Male -> DataLoader.Name.fullName "male" 
  | _ -> DataLoader.Name.fullName "female"

let makeMoney magnitude =
  let factor = 
    match magnitude with
     | Tens -> decimal 10
     | Hundreds -> decimal 100
     | Thousands -> decimal 1000
     | Millions -> decimal 1000000

  let amount = (decimal <| rng.NextDouble()) * factor
  JsonValue.Number(System.Math.Round(amount, 2))

let makeTime (range: System.DateTime option * System.DateTime option) = 
  let earliest = 
    match range with
      | (Some earliest, _) -> earliest.Ticks
      | (None, _) -> System.DateTime.MinValue.Ticks
  let latest = 
    match range with
      | (_, Some latest) -> latest.Ticks
      | (_, None) -> System.DateTime.MaxValue.Ticks

  let generatedTick = (rng.NextDouble() * double (latest - earliest)) + (double earliest)
  System.DateTime (int64 generatedTick)

let toUniversalTimeString (dateTime:System.DateTime) = 
  dateTime.ToString("yyyy-MM-ddTHH:MM:ss.FFFZ")

let extractExactly primitive = 
  match primitive with
    | Primitive.Integer i -> JsonValue.Number (decimal i)
    | Primitive.Number n -> JsonValue.Number n
    | Primitive.String s -> JsonValue.String s

let extractOneOf values =
  let primitive = Array.ofSeq values |> Array.item (rng.Next((Seq.length values)))
  extractExactly primitive

let rec runAux (mockData: MockData) : JsonValue = 
  match mockData with
    | Record entries -> 
      let results = Seq.map (fun (k, v) -> (k, runAux v)) entries
      JsonValue.Record (Array.ofSeq results)
    | Array entries -> 
      let results = [1..rng.Next(10)] |> Seq.map (fun _ -> runAux entries)
      JsonValue.Array(results |> Array.ofSeq)
    | Exactly value ->
      extractExactly value
    | OneOf values ->
      extractOneOf values
    | String -> 
      JsonValue.String (System.Guid.NewGuid() |> string)
    | Number -> 
      JsonValue.Number (decimal <| rng.NextDouble())
    | Integer -> 
      JsonValue.Number (decimal <| rng.Next())
    | Real -> 
      JsonValue.Float <| rng.NextDouble()
    | Boolean -> 
      JsonValue.Boolean (rng.NextDouble() >= 0.5)
    | Money (Some m) ->
      makeMoney m
    | Money None ->
      makeMoney Hundreds
    | Name (Some sex) ->
      let fullName = makeFullName sex
      JsonValue.String(fullName.First + " " + fullName.Last)
    | Name None ->
      let fullName = DataLoader.Name.generate ()
      JsonValue.String(fullName.First + " " + fullName.Last)
    | Location ->
      JsonValue.Record(location () |> Array.ofSeq)
    | DateTime range ->
      let dateTime = makeTime range
      JsonValue.String(toUniversalTimeString dateTime)
    | Format (fmt, spec) ->
      JsonValue.String(FormatProcessor.run fmt spec)

let run mockData =
  let toJsonValue = runAux mockData
  toJsonValue.ToString()