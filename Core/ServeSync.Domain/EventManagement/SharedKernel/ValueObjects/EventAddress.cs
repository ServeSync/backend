using GeoCoordinatePortable;
using ServeSync.Domain.SeedWorks.Models;

namespace ServeSync.Domain.EventManagement.SharedKernel.ValueObjects;

public record EventAddress : ValueObject
{
    public string FullAddress { get; private set; }
    public double Longitude { get; private set; }
    public double Latitude { get; private set; }

    internal EventAddress(string fullAddress, double longitude, double latitude)
    {
        FullAddress = Guard.NotNullOrWhiteSpace(fullAddress, nameof(FullAddress));
        Longitude = Guard.Range(longitude, nameof(Longitude), -180, 180);
        Latitude = Guard.Range(latitude, nameof(Latitude), -90, 90);
    }

    internal double DistanceTo(EventAddress other)
    {
        return new GeoCoordinate(Latitude, Longitude).GetDistanceTo(new GeoCoordinate(other.Latitude, other.Longitude));
    }
}