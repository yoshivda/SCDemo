namespace SCDemo;

public class Service(DbContext dbContext)
{
    public List<bool> DoAirportCodesExist(List<string> airportCodes)
    {
        var result = new List<bool>();
        foreach (var code in airportCodes)
        {
            var existingCodes = dbContext
                .Airport
                .Select(ap => ap.Code)
                .ToList();
            result.Add(existingCodes.Contains(code.ToUpper()));
        }

        return result;
    }

    public List<FlightPlanDto> GetFlightPlansForYear(int year)
    {
        var plans = dbContext
            .FlightPlan
            .Where(plan => plan.Departure.Year == year)
            .ToList();

        return plans
            .Select(plan => new FlightPlanDto(
                plan.Departure,
                plan.Arrival,
                plan.FlightNumber,
                plan.Plane.Airline.Name,
                plan.Plane.Model,
                plan.Plane.TailNumber,
                plan.Origin.Code,
                plan.Destination.Code))
            .ToList();
    }

    private AirportDetailsDto GetAirportDetails(string airportCode)
    {
        var airport = dbContext
            .Airport
            .Single(airport => airport.Code == airportCode);

        return new AirportDetailsDto(
            airport.Code,
            airport.Planes.Select(plane => plane.TailNumber).ToList(),
            airport.OutboundFlights.Select(flight => flight.FlightNumber).ToList(),
            airport.InboundFlights.Select(flight => flight.FlightNumber).ToList());
    }

    public List<AirportDetailsDto> GetAirportDetailsForCountry(string countryName)
    {
        var airports = dbContext
            .Airport
            .Where(airport => airport.Country == countryName)
            .ToList();
        return airports
            .Select(airport => GetAirportDetails(airport.Code))
            .ToList();
    }
}
