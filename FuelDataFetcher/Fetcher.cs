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

namespace FuelDataFetcher;

using Services.FuelInvoiceRepository;
using Services.FuelInvoiceClient;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

public class Fetcher
{
    private readonly IFuelInvoiceRepository repository;
    private readonly IFuelInvoiceClient fuelDataService;
    private readonly ILogger<Fetcher> logger;

    private readonly DateOnly date;

    public Fetcher(
        IOptions<FetcherOptions> config,
        IFuelInvoiceRepository repository,
        IFuelInvoiceClient fuelDataService,
        ILogger<Fetcher> logger)
    {
        this.repository = repository;
        this.fuelDataService = fuelDataService;
        this.logger = logger;

        this.date = config.Value.Date != null
            ? DateOnly.Parse(config.Value.Date)
            : DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
    }

    public async Task FetchAndStoreAsync()
    {
        this.logger.LogInformation("Fetching invoices for date {date}", this.date.ToString("yyyy-MM-dd"));

        var fuelData = await this.fuelDataService.FetchByDateAsync(this.date);

        if (fuelData.Count == 0)
        {
            this.logger.LogInformation("No records found");
            return;
        }

        await this.repository.OpenAsync();

        try
        {
            if (await this.repository.RecordsExist(this.date))
            {
                this.logger.LogWarning("Records already exist for {date}", this.date);
                return;
            }

            await this.repository.SaveManyAsync(fuelData);
        }
        finally
        {
            await this.repository.CloseAsync();
        }
    }
}