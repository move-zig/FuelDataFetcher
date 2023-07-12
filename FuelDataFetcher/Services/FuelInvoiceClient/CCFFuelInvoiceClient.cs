// The MIT License (MIT)
//
// Copyright 2023 Dave Welsh (davewelsh79@gmail.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

namespace FuelDataFetcher.Services.FuelInvoiceClient;

using Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;

/// <inheritdoc />
public class CCFFuelInvoiceClient : IFuelInvoiceClient
{
    /// <summary>
    /// Sent as part of the user-agent HTTP header.
    /// </summary>
    private readonly Version version = new(1, 0, 0);

    private readonly HttpClient client = new();

    private readonly string baseUrl = "https://api.ccfanalytics.com/erb_invoices";

    private readonly ILogger<CCFFuelInvoiceClient> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CCFFuelInvoiceClient"/> class.
    /// </summary>
    /// <param name="config">The configuration options.</param>
    /// <param name="logger">The logger.</param>
    public CCFFuelInvoiceClient(
        IOptions<CCFFuelInvoiceClientOptions> config,
        ILogger<CCFFuelInvoiceClient> logger)
    {
        this.logger = logger;

        this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", config.Value.AccessToken);
        this.client.DefaultRequestHeaders.Add("User-Agent", $"Erb Fuel Data Fetcher v{this.version}");
    }

    /// <inheritdoc />
    public async Task<List<FuelInvoice>> FetchByDateAsync(DateOnly date)
    {
        var ccfInvoices = await this.FetchCCFFuelInvoicesDate(date);

        return ccfInvoices.Select(this.ConvertData).ToList();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes resources.
    /// </summary>
    /// <param name="disposing">Whether to dispose of managed resources or not.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.client.Dispose();
        }
    }

    /// <summary>
    /// Fetches the raw invoices from CCF.
    /// </summary>
    /// <param name="date">The date on which to filter the invoices.</param>
    /// <returns>The invoices.</returns>
    /// <exception cref="FuelInvoiceClientException">Throws if the response deserialization returns null.</exception>
    private async Task<List<CCFFuelInvoice>> FetchCCFFuelInvoicesDate(DateOnly date)
    {
        var endpoint = this.GetEndpoint(date);

        try
        {
            var response = await this.client.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                this.logger.LogError("Unable to fetch: {reason}", response.ReasonPhrase);
                throw new FuelInvoiceClientException("Unable to fetch data");
            }

            var invoices = await response.Content.ReadFromJsonAsync<List<CCFFuelInvoice>>();

            if (invoices == null)
            {
                throw new FuelInvoiceClientException("Unable to parse JSON data");
            }

            return invoices;
        }
        catch (HttpRequestException e)
        {
            this.logger.LogError("Error making HTTP request: {message}", e.Message);
            throw;
        }
        catch (JsonException e)
        {
            this.logger.LogError("Error parsing response body: {message}", e.Message);
            throw;
        }
    }

    /// <summary>
    /// Determines the endpoint to use to filter invoices to a particular date.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <returns>The endpoint.</returns>
    private string GetEndpoint(DateOnly date)
    {
        return $"{this.baseUrl}?date=eq.{date:yyyy-MM-dd}";
    }

    /// <summary>
    /// Converts a CCFFuelInvoice into a FuelInvoice record.
    /// </summary>
    /// <param name="ccfFuelInvoice">The raw invoice from CCF.</param>
    /// <returns>A FuelInvoice record.</returns>
    private FuelInvoice ConvertData(CCFFuelInvoice ccfFuelInvoice)
    {
        return new FuelInvoice()
        {
            // TODO: supply all properties with values
            Invoice = ccfFuelInvoice.Invoice,
            Date = ccfFuelInvoice.Date,
            Time = ccfFuelInvoice.Time,
        };
    }
}