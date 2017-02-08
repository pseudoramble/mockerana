namespace Mockerana

module Constraints =
    let max n (value:string) =
        value.Substring(0, n)

    let min n value =
        let size = String.length value
        if size >= n
        then value
        else String.replicate (int(ceil((decimal n) / (decimal size)))) value

    let numeric value =
        System.Text.RegularExpressions.Regex.Replace(value, "[^0-9]", "")

    let overString value constraintType =
        match constraintType with
        | Max n -> max n value
        | Min n -> min n value
        | Numeric -> numeric value