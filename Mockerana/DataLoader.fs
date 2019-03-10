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
  let firstName sex = 
    if sex = "male"
    then "John"
    else "Jane"
    
  let lastName = "Smith"

  let fullName sex = 
    {
      First = firstName sex;
      Last = (lastName);
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
