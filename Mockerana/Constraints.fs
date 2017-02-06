namespace Mockerana

module Constraints =
    let max n mockData value =
        match mockData with
            | String -> printfn "max %d" n; mockData
            | _ -> mockData

    let min n mockData value =
        match mockData with
            | String -> printfn "min = %d" n; mockData
            | _ -> mockData

    let constrain value mockData constraintType =
        match constraintType with
        | Max n -> max n mockData value
        | Min n -> min n mockData value