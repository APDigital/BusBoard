﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BusBoard.ConsoleApp
{
  class Program
  {
    private readonly TflApi tflApi = new TflApi();
    private readonly PostcodesApi postcodesApi = new PostcodesApi();

    static void Main()
    {
      new Program().Run();
    }

    public void Run()
    {
      while (true)
      {
        var postcode = PromptForPostcode();

        var coordinate = postcodesApi.GetCoordinateForPostcode(postcode);
        var nearbyStops = tflApi.GetStopsNear(coordinate);

        foreach (var stop in nearbyStops.Take(2))
        {
          DisplayDepartureBoardForStop(stop);
        }
      }
    }

    private string PromptForPostcode()
    {
      Console.Write("Enter your postcode: ");
      return Console.ReadLine(); // Example: "NW5 1TL"
    }

    private void DisplayDepartureBoardForStop(StopPoint stop)
    {
      Console.WriteLine($"Departure board for {stop.CommonName}");

      var predictions = tflApi.GetArrivalPredictions(stop.NaptanId);
      var predictionsToDisplay = predictions.OrderBy(p => p.TimeToStation).Take(5);
      DisplayPredictions(predictionsToDisplay);

      Console.WriteLine();
    }

    private void DisplayPredictions(IEnumerable<ArrivalPrediction> predictionsToDisplay)
    {
      foreach (var prediction in predictionsToDisplay)
      {
        Console.WriteLine($"{prediction.TimeToStation/60} minutes: {prediction.LineName} to {prediction.DestinationName}");
      }
    }
  }
}
