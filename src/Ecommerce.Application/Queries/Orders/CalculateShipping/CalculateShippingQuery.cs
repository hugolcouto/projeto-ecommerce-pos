using System;
using Ecommerce.Core.Entities;
using Microsoft.VisualBasic;

namespace Ecommerce.Application;

public class CalculateShippingQuery
{
    public string ZipCode { get; set; } = string.Empty;
    public List<CalculateShippingQueryItem> Items { get; set; } = [];
}
