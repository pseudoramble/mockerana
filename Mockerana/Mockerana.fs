namespace Mockerana

type ValueConstraint<'a> =
    | Max of int
    | Min of int

type MockData = 
    | String
    | Integer
    | Real
    | Number
    | Boolean
    | Money
    | Constrained of MockData * ValueConstraint<MockData> seq
    | Array of MockData
    | Record of (string * MockData) seq