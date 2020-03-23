module DataLoader

open FSharp.Data
let rng = System.Random()

type Location = 
  {
    address: string;
    city: string;
    state: string;
    zip: string
  }

type Name =
  {
    First: string;
    Last: string;
  }

module Name =
  let femaleNamesDb = CsvFile.Load "data/female_firstnames.csv"
  let maleNamesDb = CsvFile.Load "data/male_firstnames.csv"
  let lastNamesDb = CsvFile.Load "data/lastnames.csv"

  let firstName sex = 
    let row = 
      match sex with
        | "male" ->
          Array.ofSeq (maleNamesDb.Rows)
          |> Array.item (rng.Next(Seq.length maleNamesDb.Rows))
        | _ ->
          Array.ofSeq (femaleNamesDb.Rows)
          |> Array.item (rng.Next(Seq.length femaleNamesDb.Rows))
    let name = (row.Item 0)
    sprintf "%s%s" (name.Substring(0, 1).ToUpper()) (name.Substring(1).ToLower())
    
  let lastName () =
    let row = 
      Array.ofSeq (lastNamesDb.Rows)
      |> Array.item (rng.Next(Seq.length lastNamesDb.Rows))
    let name = (row.Item 0)
    sprintf "%s%s" (name.Substring(0, 1).ToUpper()) (name.Substring(1).ToLower())

  let fullName sex = 
    {
      First = firstName sex;
      Last = lastName ();
    }

  let generate () =
    let sex = 
      if rng.NextDouble() <= 0.5
      then "male"
      else "female"
    
    fullName sex

module Location =
  let locationDb = CsvFile.Load "data/zip_code_database.csv"
  let streetNameDb = CsvFile.Load "data/streets.csv"
  
  let address = 
      let result =
        Array.ofSeq (streetNameDb.Rows)
        |> Array.item (rng.Next(Seq.length streetNameDb.Rows))
      
      sprintf "%d %s" (rng.Next(9999)) (result.Item 0)

  let formatZip zipCode = 
    if zipCode < 9999
    then sprintf "0%d" zipCode
    else string zipCode

  let generate () =
    let chosenLocation =
      Array.ofSeq (locationDb.Rows)
      |> Array.item (rng.Next(Seq.length locationDb.Rows))
    
    {
      address = (address)
      state = chosenLocation.Item 5; // State, 6th column
      zip = formatZip (int <| chosenLocation.Item 0); // Zip code. 1st column
      city = chosenLocation.Item 2; // Primary City. 3rd column
    }
