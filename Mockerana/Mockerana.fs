namespace Mockerana

type ValueConstraint =
    | Max of int
    | Min of int
    | Numeric

type MockData = 
    | String
    | Integer
    | Real
    | Number
    | Boolean
    | Money
    | Location
    | Array of MockData
    | Record of (string * MockData) seq