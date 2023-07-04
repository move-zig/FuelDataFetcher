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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Services.FuelInvoiceClient;
using Services.FuelInvoiceRepository;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        ConfigureServices(builder);

        var host = builder.Build();

        var fetcher = host.Services.GetRequiredService<Fetcher>();
        await fetcher.RunAsync();
    }

    private static void ConfigureServices(IHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            var configuration = context.Configuration;

            services.AddOptions<CCFFuelInvoiceClientOptions>()
               .Bind(configuration.GetSection(CCFFuelInvoiceClientOptions.ConfigurationSectionName))
               .ValidateDataAnnotations();

            services.AddTransient<IFuelInvoiceClient, CCFFuelInvoiceClient>();

            services.AddTransient<IFuelInvoiceRepository, MSSQLFuelInvoiceRepository>();

            services.AddScoped<Fetcher>();
        });
    }
}
