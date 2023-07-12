namespace FuelDataFetcher;

public sealed class FetcherOptions
{
    public const string ConfigurationSectionName = "Fetcher";

    public string? Date { get; set; } = null;
}