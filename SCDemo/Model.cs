namespace SCDemo;

using System.ComponentModel.DataAnnotations.Schema;

public class Plane(string model, string tailNumber)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Model { get; init; } = model;
    public string TailNumber { get; init; } = tailNumber;
    public virtual Airport BaseAirport { get; set; }
    public virtual ICollection<FlightPlan> FlightPlans { get; init; } = [];
    public virtual Airline Airline { get; set; }
}

public class Airline(string code, string name)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Code { get; init; } = code;
    public string Name { get; init;  } = name;
}

public class FlightPlan(DateTime departure, DateTime arrival, string flightNumber)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string FlightNumber { get; init; } = flightNumber;
    public DateTime Departure { get; init; } = departure;
    public DateTime Arrival { get; init; } = arrival;

    [InverseProperty(nameof(Plane.FlightPlans))]
    public virtual required Plane Plane { get; init; }
    public virtual required Airport Origin { get; init; }
    public virtual required Airport Destination { get; init; }
}

public class Airport(string code, string country)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Code { get; init; } = code;
    public string Country { get; init; } = country;

    [InverseProperty(nameof(Plane.BaseAirport))]
    public virtual ICollection<Plane> Planes { get; } = [];

    [InverseProperty(nameof(FlightPlan.Origin))]
    public virtual ICollection<FlightPlan> OutboundFlights { get; } = [];

    [InverseProperty(nameof(FlightPlan.Destination))]
    public virtual ICollection<FlightPlan> InboundFlights { get; } = [];
}
