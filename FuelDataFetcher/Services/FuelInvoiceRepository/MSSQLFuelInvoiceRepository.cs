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

namespace FuelDataFetcher.Services.FuelInvoiceRepository;

using Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <inheritdoc/>
internal class MSSQLFuelInvoiceRepository : IFuelInvoiceRepository
{
    private readonly SqlConnection connection;

    private readonly string tableName = "fuel_invoices";

    private readonly ILogger<MSSQLFuelInvoiceRepository> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MSSQLFuelInvoiceRepository"/> class.
    /// </summary>
    /// <param name="config">The configuration options.</param>
    /// <param name="logger">The logger.</param>
    public MSSQLFuelInvoiceRepository(
        IOptions<MSSQLFuelInvoiceRepositoryOptions> config,
        ILogger<MSSQLFuelInvoiceRepository> logger)
    {
        this.logger = logger;

        this.connection = new SqlConnection(config.Value.ConnectionString);
    }

    /// <inheritdoc />
    public void Open()
    {
        this.connection.Open();
    }

    /// <inheritdoc />
    public async Task OpenAsync()
    {
        await this.connection.OpenAsync();
    }

    /// <inheritdoc />
    public void Close()
    {
        this.connection.Close();
    }

    /// <inheritdoc />
    public async Task CloseAsync()
    {
        await this.connection.CloseAsync();
    }

    /// <inheritdoc />
    public async Task SaveManyAsync(IEnumerable<FuelInvoice> invoices)
    {
        var sql = $"INSERT INTO {this.tableName} SET invoice = :invoice";

        try
        {
            using var command = new SqlCommand(sql, this.connection);

            // TODO: add all required parameters
            command.Parameters.Add(new SqlParameter("invoice", SqlDbType.VarChar));

            foreach (var invoice in invoices)
            {
                // TODO: set each parameter's value
                command.Parameters["invoice"].Value = invoice.Id;

                await command.ExecuteNonQueryAsync();
            }
        }
        catch (SqlException e)
        {
            this.logger.LogError("Database error {message}", e.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.Dispose(disposing: true);
    }

    /// <summary>
    /// Disposes resources.
    /// </summary>
    /// <param name="disposing">Whether to dispose of managed resources or not.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.connection.Dispose();
        }
    }
}
