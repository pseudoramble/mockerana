namespace Mockerana

module FormatProcessor = 
  open System.Text.RegularExpressions
  let rng = System.Random()

  let extractBounds constraints = 
    let minimum = 
      match Map.tryFind "min" constraints with
        | Some (Primitive.Number p) -> int p
        | Some (_) | None -> 0

    let maximum =
      match Map.tryFind "max" constraints with
        | Some (Primitive.Number p) -> int p
        | Some (_) | None -> System.Int32.MaxValue

    (minimum, maximum)

  let generateInt constraints =
    let (minimum, maximum) = extractBounds constraints
    rng.Next(minimum, maximum)

  let generateNumber constraints =
    let (minimum, maximum) = extractBounds constraints
    (rng.NextDouble() * double (maximum - minimum)) + (double minimum)

  let generateStr constraints = 
    let (_, maximum) = extractBounds constraints
    let result = System.Guid.NewGuid() |> string
    result.Substring(0, maximum)

  let generateBool constraints =
    let bias = 
      match Map.tryFind "bias" constraints with
        | Some (Primitive.Number p) -> double p
        | Some _ | None -> double 0.5

    rng.NextDouble() >= bias

  let generate mockData constraints =
    match mockData with
      | MockData.Boolean -> generateBool constraints |> string
      | MockData.Integer -> generateInt constraints |> string
      | MockData.Number -> generateNumber constraints |> string
      | MockData.String -> generateStr constraints
      | _ -> ""
  
  let rollFormat (fmt: string) (values: string * string) =
    let (that, this) = values
    fmt.Replace(this, that)

  let run (fmt: string) (spec: ConstrainedData seq) =
    let replaceWith = Seq.map (fun (mockData, constraints) -> generate mockData constraints) spec
    let replacementSpots = Regex.Matches(fmt, "{%([a-zA-Z0-9=:;]+)%}") |> Seq.cast<Match> |> Seq.map (fun m -> m.Value)
    
    let finalResult = 
      Seq.zip replaceWith replacementSpots
      |> Seq.fold rollFormat fmt
    
    finalResult