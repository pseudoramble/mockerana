module DataLoader
  open FSharp.Data
  let rng = new System.Random()

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
    type ZipCodeDb = CsvProvider<"../data/zip_code_database.csv">
    type StreetNameDb = CsvProvider<"../data/streets.csv">

    let locationDb = ZipCodeDb.GetSample()
    let streetNameDb = StreetNameDb.GetSample()
    
    let address = 
        let result =
          Array.ofSeq (streetNameDb.Rows)
          |> Array.item (rng.Next(Seq.length streetNameDb.Rows))

        sprintf "%d %s" (rng.Next(9999)) result.Street

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
        state = chosenLocation.State;
        zip = formatZip chosenLocation.Zip;
        city = chosenLocation.Primary_city;
      }
