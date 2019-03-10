module Mockerana

type Gender =
  | Male
  | Female

type Magnitude =
  | Tens
  | Hundreds
  | Thousands
  | Millions

type Primitive =
  | Integer of int
  | Number of decimal
  | String of string

type ConstraintsMap = Map<string, Primitive>

type MockData = 
  | String
  | Integer
  | Real
  | Number
  | Boolean
  | Location
  | Format of (string * ConstrainedData seq)
  | DateTime of (System.DateTime option * System.DateTime option)
  | Exactly of Primitive
  | OneOf of Primitive seq
  | Money of Magnitude option
  | Name of Gender option
  | Array of MockData
  | Record of (string * MockData) seq
and ConstrainedData = (MockData * ConstraintsMap)

module Supports =
  module Format =
    open System.Text.RegularExpressions

    let removeEmptyValues values = 
      Seq.filter (fun x -> x <> "") values

    let toConstraintList (input: string) = 
      input.Split(';') |> Seq.ofArray

    let toPairs (constraintPair: string) = 
      constraintPair.Split('=') |> Seq.pairwise

    let toMockData (input: string) = 
      match input with
        | "bool" -> MockData.Boolean
        | "int" -> MockData.Integer
        | "num" -> MockData.Number
        | "str" -> MockData.String
        | _ -> MockData.String

    let toPrimitiveMockData (givenType: string) (value: string) =
      match givenType with
        | "len" -> Primitive.Integer (int value)
        | "max" | "min" -> Primitive.Number (decimal value)
        | _ -> Primitive.String value

    let toConstraintPair constraints =
      Seq.collect toPairs constraints
      |> Seq.map (fun (t, v) -> (t, (toPrimitiveMockData t v)))

    let findWrittenConstraints input = 
      Regex.Match(input, ":(.*)$").Groups.[1].Value

    let findWrittenType input = 
      Regex.Match(input, "([a-zA-Z]+):?").Groups.[1].Value

    let create fmt =
      let patterns = Regex.Matches(fmt, "{%([a-zA-Z0-9=:;]+)%}") 
                      |> Seq.cast<Match> 
                      |> Seq.map (fun m -> m.Groups.[1].Value)
      let types = patterns
                    |> Seq.map (findWrittenType >> toMockData)
      let constraints = patterns
                          |> Seq.map (findWrittenConstraints >> toConstraintList >> removeEmptyValues >> toConstraintPair)

      let constraintsMap = Seq.map Map.ofSeq constraints
      let results = (Seq.zip types constraintsMap)

      Format (fmt, results)