namespace Mockerana

type ValueConstraint =
    | Max of int
    | Min of int
    | Numeric

type Gender =
    | Male
    | Female

type MockData = 
    | String
    | Integer
    | Real
    | Number
    | Boolean
    | Money
    | Location
    | Name of Gender option
    | Array of MockData
    | Record of (string * MockData) seq