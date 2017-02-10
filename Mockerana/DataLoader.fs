module DataLoader
  open FSharp.Data

  type ZipCodeDb = CsvProvider<"../data/zip_code_database.csv">
  type StreetNameDb = CsvProvider<"../data/streets.csv">

  type Location = 
    {
      address: string;
      city: string;
      state: string;
      zip: string
    }

  let rng = new System.Random()

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

  let location () =
    let chosenLocation =
      Array.ofSeq (locationDb.Rows)
      |> Array.item (rng.Next(Seq.length locationDb.Rows))

    {
      address = (address)
      state = chosenLocation.State;
      zip = formatZip chosenLocation.Zip;
      city = chosenLocation.Primary_city;
    }
