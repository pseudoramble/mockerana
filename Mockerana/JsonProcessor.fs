namespace Mockerana

open FSharp.Data

module JsonProcessor =
  let rec runAux (mockData: MockData) : JsonValue = 
      match mockData with
      | Record entries -> 
          let results = Seq.map (fun (k, v) -> (k, runAux v)) entries
          JsonValue.Record (Array.ofSeq results)
      | Array entries -> FSharp.Data.JsonValue.Array(Seq.map runAux entries |> Array.ofSeq)
      | String -> JsonValue.String "a made up string value"
      | Number -> JsonValue.Number (decimal 4.56)
      | Integer -> JsonValue.Number (decimal 42)
      | Real -> JsonValue.Float 1.23
      | Boolean -> JsonValue.Boolean false
      | Constrained (mockDataType, constraints) -> 
          JsonValue.String "Constraints not yet supported"

  let run mockData =
    let toJsonValue = runAux mockData
    toJsonValue.ToString()