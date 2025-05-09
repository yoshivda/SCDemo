namespace SCDemo;

public record FlightPlanDto(
    DateTime Departure,
    DateTime Arrival,
    string FlightNumber,
    string Airline,
    string PlaneModel,
    string PlaneTailNumber,
    string OriginAirport,
    string DestinationAirport);

public record AirportDetailsDto(
    string Code,
    List<string> AirplaneTailNumbers,
    List<string> OutboundFlightNumbers,
    List<string> InboundFlightNumbers);
