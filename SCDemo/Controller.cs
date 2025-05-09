namespace SCDemo;

using Microsoft.AspNetCore.Mvc;

[ApiController]
public class Controller(Service service) : ControllerBase
{
    [HttpGet("flightplan/{year:int}", Name = "FlightPlans")]
    public IEnumerable<FlightPlanDto> GetFlightPlans(int year)
    {
        return service.GetFlightPlansForYear(year);
    }

    [HttpGet("airport/exist", Name = "AirportCodesExist")]
    public List<bool> DoAirportCodesExist([FromQuery] List<string> airportCodes)
    {
        return service.DoAirportCodesExist(airportCodes);
    }

    [HttpGet("airport/country/{country}", Name = "AirportDetailsForCountry")]
    public List<AirportDetailsDto> GetAirportDetailsForCountry(string country)
    {
        return service.GetAirportDetailsForCountry(country);
    }
}
