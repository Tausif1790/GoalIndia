using System;
using Core.Entities;

namespace Core.Specifications;

// Responsible for filtering, sorting, and selecting products based on various criteria
public class ProductSpecification : BaseSpecification<Product>
{   
    // ProductSpecification has all properties => Criteria,OrderBy,OrderByDescending (from base calss)
    // constructor
    public ProductSpecification(string? brand, string? type, string? sort) : base(x =>
        (string.IsNullOrWhiteSpace(brand) || x.Brand == brand) &&    // Filters products by brand if provided
        (string.IsNullOrWhiteSpace(type) || x.Type == type)          // Filters products by type if provided
    )
    {
        switch (sort)                                                // Adds sorting based on the provided criteria
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);                            // Sorts products by price in ascending order
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);                  // Sorts products by price in descending order
                break;
            default:
                AddOrderBy(x => x.Name);                             // Default sorting by product name
                break;
        }
    }
}
