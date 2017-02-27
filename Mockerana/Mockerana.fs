namespace Mockerana

type ValueConstraint =
    | Max of int
    | Min of int
    | Numeric

type Gender =
    | Male
    | Female

type Magnitude =
    | Tens
    | Hundreds
    | Thousands
    | Millions

type Primitive =
    | String of string
    | Number of decimal

type MockData = 
    | String
    | Integer
    | Real
    | Number
    | Boolean
    | Location
    | DateTime of (System.DateTime option * System.DateTime option)
    | Exactly of Primitive
    | OneOf of Primitive seq
    | Money of Magnitude option
    | Name of Gender option
    | Array of MockData
    | Record of (string * MockData) seq