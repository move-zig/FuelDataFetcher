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

using System.Text.Json.Serialization;

public record CCFFuelInvoice
{
    /// <summary>Gets or sets the ticket number on the invoice</summary>
    [JsonPropertyName("ticket")]
    required public string Ticket { get; set; }

    /// <summary>Gets or sets the invoice number.</summary>
    [JsonPropertyName("invoice")]
    required public string Invoice { get; set; }

    /// <summary>Gets or sets the public code of the location being delivered to.</summary>
    [JsonPropertyName("ship_to")]
    required public string ShipTo { get; set; }

    /// <summary>Gets or sets the name of the location being delivered to.</summary>
    [JsonPropertyName("ship_to_desc")]
    required public string ShipToDescription { get; set; }

    /// <summary>Gets or sets the street address of the location being delivered to.</summary>
    [JsonPropertyName("ship_to_addr")]
    required public string ShipToAddress { get; set; }

    /// <summary>Gets or sets the date of the delivery.</summary>
    [JsonPropertyName("date")]
    required public DateOnly Date { get; set; }

    /// <summary>Gets or sets the time of delivery represented in HH:mm:ss format.</summary>
    [JsonPropertyName("time")]
    required public TimeOnly Time { get; set; }

    /// <summary>Gets or sets the PO box number.</summary>
    [JsonPropertyName("po_num")]
    required public string POBoxNumber { get; set; }

    /// <summary>Gets or sets the short code for the fuel type or charge.</summary>
    [JsonPropertyName("product_num")]
    required public string ProductNumber { get; set; }

    /// <summary>Gets or sets the long description of the fuel type or charge.</summary>
    [JsonPropertyName("product_desc")]
    required public string ProductDescription { get; set; }

    /// <summary>Gets or sets the public ID for equipment fuel is being delivered to.</summary>
    [JsonPropertyName("unit_num")]
    required public string UnitNumber { get; set; }

    /// <summary>Gets or sets the barcode for equipment fuel is being delivered to.</summary>
    [JsonPropertyName("unit_barcode")]
    required public string UnitBarcode { get; set; }

    /// <summary>Gets or sets the total amount of product.</summary>
    [JsonPropertyName("quantity")]
    required public float Quantity { get; set; }

    /// <summary>Gets or sets the unit price without tax.</summary>
    [JsonPropertyName("unit_price_less_taxes")]
    required public float UnitPriceLessTaxes { get; set; }

    /// <summary>Gets or sets the federal tax.</summary>
    [JsonPropertyName("fet")]
    required public float FederalTax { get; set; }

    /// <summary>Gets or sets the provincial tax.</summary>
    [JsonPropertyName("pft")]
    required public float ProvincialTax { get; set; }

    /// <summary>Gets or sets the carbon tax.</summary>
    [JsonPropertyName("ctx")]
    required public float CarbonTax { get; set; }

    /// <summary>Gets or sets the sales tax.</summary>
    [JsonPropertyName("hst")]
    required public float HarmonizedSalesTax { get; set; }

    /// <summary>Gets or sets the unit price with tax.</summary>
    [JsonPropertyName("total_price")]
    required public float TotalPrice { get; set; }
}