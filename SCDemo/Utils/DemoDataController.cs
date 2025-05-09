namespace SCDemo;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

[ApiController]
public class DummyDataController(DbContext dbContext) : ControllerBase
{
    private static readonly Random Random = new();

    [HttpPost("createDemoData", Name = "CreateDemoData")]
    public void CreateDemoData()
    {
        var planesJson = System.IO.File.ReadAllText("Resources/planes.json");
        var airportJson = System.IO.File.ReadAllText("Resources/airports.json");
        var airlineJson = System.IO.File.ReadAllText("Resources/airlines.json");

        // Deserialize
        var planes = JsonSerializer.Deserialize<List<Plane>>(planesJson)!;
        var airportData = JsonSerializer.Deserialize<Dictionary<string, string>>(airportJson)!.ToHashSet();
        var airlineData = JsonSerializer.Deserialize<Dictionary<string, string>>(airlineJson)!;

        var airports = airportData.Select(kv => new Airport(kv.Key, kv.Value)).ToList();
        var airlines = airlineData.Select(kv => new Airline(kv.Key, kv.Value)).ToList();

        var ams = airports.Single(ap => ap.Code == "AMS");
        var klm = airlines.Single(al => al.Code == "KL");
        foreach (var plane in planes.Take(50))
        {
            plane.BaseAirport = ams;
            plane.Airline = klm;
        }

        foreach (var plane in planes.Skip(50))
        {
            plane.BaseAirport = airports[Random.Next(airports.Count)];
            plane.Airline = airlines[Random.Next(airlines.Count)];
        }

        List<FlightPlan> flightPlans = [];
        foreach (var _ in Enumerable.Range(0, 5000))
        {
            var departure =
                GetRandomDateTime(new DateTime(Random.Next(2019, 2025), Random.Next(1, 13), Random.Next(1, 28)));
            var arrival = GetRandomDateTime(departure.AddMinutes(30));
            var planAirports = airports.OrderBy(_ => Random.Next()).Take(2).ToList();
            var plane = planes[Random.Next(planes.Count)];
            var flightNumber = $"{plane.Airline.Code}{Random.Next(1000, 9999)}";
            flightPlans.Add(
                new FlightPlan(departure, arrival, flightNumber)
                {
                    Plane = plane,
                    Origin = planAirports.First(),
                    Destination = planAirports.Last()
                });
        }

        var extraAirportCodes = new HashSet<string>();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var airportCodes = airports.Select(ap => ap.Code).ToHashSet();
        while (extraAirportCodes.Count < 5000)
        {
            var code = new string(new[]
            {
                chars[Random.Next(26)],
                chars[Random.Next(26)],
                chars[Random.Next(26)]
            });

            if (!airportCodes.Contains(code))
            {
                extraAirportCodes.Add(code);
            }
        }
        dbContext.Airport.AddRange(extraAirportCodes.Select(code => new Airport(code, "NonExisting")));
        dbContext.Plane.AddRange(planes);
        dbContext.FlightPlan.AddRange(flightPlans);
        dbContext.SaveChanges();
    }

    [HttpPost("deleteDemoData", Name = "DeleteAlldata")]
    public void DeleteAllData()
    {
        dbContext.FlightPlan.ExecuteDelete();
        dbContext.Airport.ExecuteDelete();
        dbContext.Plane.ExecuteDelete();
    }

    private static DateTime GetRandomDateTime(DateTime minValue)
    {
        var range = Math.Min((int)(DateTime.Today - minValue).TotalSeconds, 80_000);
        return minValue.AddSeconds(Random.Next(range));
    }
}
