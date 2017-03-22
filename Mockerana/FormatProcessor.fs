namespace Mockerana

module FormatProcessor = 
  open System.Text.RegularExpressions
  let rng = System.Random()

  let generateInt constraints =
    let minimum = 
      match Map.tryFind "min" constraints with
        | Some (Primitive.Number p) -> int p
        | Some (_) | None -> 0

    let maximum =
      match Map.tryFind "max" constraints with
        | Some (Primitive.Number p) -> int p
        | Some (_) | None -> System.Int32.MaxValue

    rng.Next(minimum, maximum)

  let generate mockData constraints =
    match mockData with
    | MockData.Boolean -> rng.NextDouble() >= 0.5 |> string
    | MockData.Integer -> generateInt constraints |> string
    | MockData.Number -> rng.NextDouble() |> string
    | MockData.String -> System.Guid.NewGuid() |> string
    | _ -> ""
  
  let rollFormat (fmt: string) (values: string * string) =
    let (replaceWith, replaceSpot) = values
    ("", fmt.Replace(replaceSpot, replaceWith))

  let run (fmt: string) (spec: ConstrainedData seq) =
    let replaceWith = Seq.map (fun (mockData, constraints) -> generate mockData constraints) spec
    let replacementSpots = Regex.Matches(fmt, "{%([a-zA-Z0-9=:;]+)%}") |> Seq.cast<Match> |> Seq.map (fun m -> m.Value)
    
    let (_, finalResult) = 
      Seq.zip replaceWith replacementSpots
      |> Seq.mapFold rollFormat fmt
    
    finalResult