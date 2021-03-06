# Mockerana

Sometimes you just don't want to write mock data by hand.
Sometimes you'd rather just be doing your favorite early-90's dance instead of that.

That's what Mockerana is here to help with. It is a data-generation tool with a few goals in mind:

* Make creating mock data simpler by letting the specifics be done by the tool.
  * Provide a schema-like type to the JsonProcessor, and get the JSON output back.
* Make mock data that is realistic looking.
  * Believable names, real-ish looking street addresses, etc.
* Allow for a little control over what data is shown

## Example

Given this:

```
let order = Record [
 ("name", String);
 ("total", Money)
 ("location", Location);
 ("steps", Array(
   Record [
     ("amount", Money)
     ("processed", Boolean)
   ]
 ))
]
```

Processing this using the `JsonProcessor` like this `JsonProcess.run order` you will receive output similar to this:

```
{
  "name": "3a0a468e-dd72-4869-906c-59f160715d14",
  "total": 0.509364328118676,
  "location": {
    "address": "3738 Country Club Road",
    "city": "Baltimore",
    "state": "MD",
    "zip": "21202"
  },
  "steps": [
    {
      "amount": 0.535685582335892,
      "processed": true
    },
    {
      "amount": 0.773205705812762,
      "processed": true
    }
  ]
}
```

## Demo

To run the demo project, be sure to install the latest version of .NET Core and the CLI.

To run: `dotnet run --project Mockerana.Demo`. The demo tries to provide an example of each available `MockData` type, including constraints that can be applied to the types. This isn't 100% complete, but should provide a decent reference on making your own specs.