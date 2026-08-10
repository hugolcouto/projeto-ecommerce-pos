namespace Ecommerce.Infrastructure.Geolocation;

public interface IGeolocationService
{
    Task<int> GetDistance(string origin, string destination);
}
