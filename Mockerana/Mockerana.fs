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

type MockData = 
    | String
    | Integer
    | Real
    | Number
    | Boolean
    | Location
    | Money of Magnitude option
    | Name of Gender option
    | Array of MockData
    | Record of (string * MockData) seq