namespace SCDemo;

using Microsoft.EntityFrameworkCore;

public class DbContext(DbContextOptions<DbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Plane> Plane { get; init; }
    public DbSet<FlightPlan> FlightPlan { get; init; }
    public DbSet<Airport> Airport { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FlightPlan>()
            .HasOne(fp => fp.Plane)
            .WithMany(plane => plane.FlightPlans)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FlightPlan>()
            .HasOne(fp => fp.Origin)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<FlightPlan>()
            .HasOne(fp => fp.Destination)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Airport>()
            .HasMany(ap => ap.InboundFlights)
            .WithOne(fp => fp.Destination)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Airport>()
            .HasMany(ap => ap.OutboundFlights)
            .WithOne(fp => fp.Origin)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
