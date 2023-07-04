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

internal record CCFFuelInvoice
{
    /// <summary>Gets or sets the ticket number on the invoice</summary>
    required public string ticket { get; set; }

    /// <summary>Gets or sets the invoice number.</summary>
    required public string invoice { get; set; }

    /// <summary>Gets or sets the internal code of the location being delivered to.</summary>
    required public string ship_to { get; set; }

    /// <summary>Gets or sets the name of the location being delivered to.</summary>
    required public string ship_to_desc { get; set; }

    /// <summary>Gets or sets the street address of the location being delivered to.</summary>
    required public string ship_to_addr { get; set; }

    /// <summary>Gets or sets the date of the delivery.</summary>
    required public DateOnly date { get; set; }

    /// <summary>Gets or sets the time of delivery represented as an int in 24-hour format.</summary>
    required public int time { get; set; }

    /// <summary>Gets or sets the PO box number.</summary>
    required public string po_num { get; set; }

    /// <summary>Gets or sets the short code for the fuel type or charge.</summary>
    required public string product_num { get; set; }

    /// <summary>Gets or sets the long description of the fuel type or charge.</summary>
    required public string product_desc { get; set; }

    /// <summary>Gets or sets the internal ID for equipment fuel is being delivered to.</summary>
    required public string unit_num { get; set; }

    /// <summary>Gets or sets the barcode for equipment fuel is being delivered to.</summary>
    required public string unit_barcode { get; set; }

    /// <summary>Gets or sets the total amount of product.</summary>
    required public float quantity { get; set; }

    /// <summary>Gets or sets the unit price without tax.</summary>
    required public float unit_price_less_taxes { get; set; }

    /// <summary>Gets or sets the federal tax.</summary>
    required public float fet { get; set; }

    /// <summary>Gets or sets the provincial tax.</summary>
    required public float pft { get; set; }

    /// <summary>Gets or sets the carbon tax.</summary>
    required public float ctx { get; set; }

    /// <summary>Gets or sets the sales tax.</summary>
    required public float hst { get; set; }

    /// <summary>Gets or sets the unit price with tax.</summary>
    required public float total_price { get; set; }
}