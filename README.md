# Software Craftsmanship 5: Performance - Demo Project

This project contains a DotNet web server using a local database through EF Core.

## Setup

1. Clone this repository.
2. Create a local database using the following command in the terminal of your IDE: `sqllocaldb create sc1`
3. Run the application. The database schemas will be set up by the application, and Swagger will automatically open in your web browser.
4. Through Swagger (or your tool of choice), call the `CreateDemoData` endpoint to populate the database.

## What to do

The [`Service` class](SCDemo/Service.cs) contains the logic for the 3 API endpoints.
All methods have suboptimal implementations.
It is your task to optimise this code.
No changes are necessary outside the `Service` class.

Notes:
- You can see the generated SQL queries by changing the log level of "Microsoft.EntityFrameworkCore" to "Information" in [appsettings.json](SCDemo/appsettings.json) (requires restarting the application).
- For the `AirportDetailsForCountry` endpoint: use "Netherlands", as the demo data is constructed to have at least 50 planes with Schiphol as base airport.
- For each request, statistics such as response time and database queries are logged automatically.
- To simulate using a remote database, 10ms of artificial latency is added to each database query.
