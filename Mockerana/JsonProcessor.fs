namespace Mockerana

open FSharp.Data

module JsonProcessor =
  let rng = new System.Random()

  let location =
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
    let factor = match magnitude with
                    | Tens -> decimal 10
                    | Hundreds -> decimal 100
                    | Thousands -> decimal 1000
                    | Millions -> decimal 1000000

    let amount = (decimal <| rng.NextDouble()) * factor
    JsonValue.Number(System.Math.Round(amount, 2))

  let rec runAux (mockData: MockData) : JsonValue = 
    match mockData with
    | Record entries -> 
        let results = Seq.map (fun (k, v) -> (k, runAux v)) entries
        JsonValue.Record (Array.ofSeq results)
    | Array entries -> 
        let results = [1..rng.Next(10)] |> Seq.map (fun _ -> runAux entries)
        FSharp.Data.JsonValue.Array(results |> Array.ofSeq)
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
        JsonValue.Record((location) |> Array.ofSeq)

  let run mockData =
    let toJsonValue = runAux mockData
    toJsonValue.ToString()