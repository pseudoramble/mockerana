namespace Mockerana

open FSharp.Data

module JsonProcessor =
  let rng = new System.Random()

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
    | Money -> 
        JsonValue.Number <| decimal (System.Math.Round((rng.NextDouble() * (float <| rng.Next())), 2))
    | Boolean -> 
        JsonValue.Boolean (rng.NextDouble() >= 0.5)
    | Constrained (mockDataType, constraints) -> 
        runConstrainers mockDataType constraints

  and runConstrainers mockDataType constraints = 
    let initialValue = runAux mockDataType

    match mockDataType with
     | String -> 
        let unboxed = match initialValue with JsonValue.String s -> s | _ -> ""
        let results = Seq.fold (Constraints.overString) unboxed constraints
        JsonValue.String results
     | _ -> invalidOp "Constraints not supported on this type at this time"

  let run mockData =
    let toJsonValue = runAux mockData
    toJsonValue.ToString()