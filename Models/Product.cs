﻿namespace PSProductService.Models;

public class Product
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Vendor { get; set; }
    public string ImageUrl { get; set; }
}