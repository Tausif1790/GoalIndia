using System;

namespace Core.Entities;

public class Product : BaseEntity
{
    public required string Name { get; set; }       // for entity class  put always this as public
    public required string Description { get; set; }    // required => not null
    public decimal Price { get; set; }
    public required string PictureUrl { get; set; }
    public required string Type { get; set; }
    public required string Brand { get; set; }
    public int QuantityInStock { get; set; }
}
