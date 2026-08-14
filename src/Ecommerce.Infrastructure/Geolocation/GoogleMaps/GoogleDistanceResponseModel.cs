namespace Ecommerce.Infrastructure.Geolocation.GoogleMaps;

public class GoogleDistanceResponseModel
{
    public string[] destination_addresses { get; set; } = default!;
    public string[] origin_addresses { get; set; } = default!;
    public Rows[] rows { get; set; } = default!;
    public string status { get; set; } = default!;
}

public class Rows
{
    public Elements[] elements { get; set; } = default!;
}

public class Elements
{
    public Distance distance { get; set; } = default!;
    public Duration duration { get; set; } = default!;
    public string status { get; set; } = default!;
}

public class Distance
{
    public string text { get; set; } = default!;
    public int value { get; set; }
}

public class Duration
{
    public string text { get; set; } = default!;
    public int value { get; set; }
}
