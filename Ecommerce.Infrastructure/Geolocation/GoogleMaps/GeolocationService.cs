using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace Ecommerce.Infrastructure.Geolocation;

public class GeolocationService : IGeolocationService
{
    private readonly GeolocationSettings _geolocationSettings;

    public GeolocationService(IOptions<GeolocationSettings> options)
    {
        _geolocationSettings = options.Value;
    }

    public async Task<int> GetDistance(string origin, string destination)
    {
        HttpClient client = new();

        HttpRequestMessage request = new(
            HttpMethod.Get,
            $"{_geolocationSettings.Api}?destinations={destination}&origins={origin}&units=metric&key={_geolocationSettings.Key}"
        );

        HttpResponseMessage? response = await client.SendAsync(request);

        var responseModel = await response.Content.ReadFromJsonAsync<GoogleDistanceResponseModel>();

        if (responseModel is null)
            return -1;

        int distanceKm = responseModel.rows[0].elements[0].distance.value / 1000;

        return distanceKm;
    }
}
