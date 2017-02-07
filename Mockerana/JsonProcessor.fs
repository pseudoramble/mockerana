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
            JsonValue.String "a made up string value"
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
            JsonValue.String "Constraints not yet supported"

  let run mockData =
    let toJsonValue = runAux mockData
    toJsonValue.ToString()