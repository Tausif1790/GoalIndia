using Core.Entities;

namespace Core.Specifications;

// The ProductSpecification class is a concrete implementation of the BaseSpecification<Product>,
// which applies filtering, sorting, and pagination to products based on parameters provided via
// the ProductSpecParams object. It uses the specification pattern to build complex queries in a reusable manner.
public class ProductSpecification : BaseSpecification<Product>
{   
    // ProductSpecification has all properties => Criteria,OrderBy,OrderByDescending (from base calss)
    // constructor // use ProductSpacParams object instead of all parameters(string? brand, string? type, string? sort)
    public ProductSpecification(ProductSpacParams specParams) : base(x =>                               // Sending "Expression" to base constructor
    (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&        // Search products by brand if provided
        (specParams.Brands.Count == 0 || specParams.Brands.Contains(x.Brand)) &&    // Filters products by brand if provided
        (specParams.Types.Count == 0 || specParams.Types.Contains(x.Type))          // Filters products by type if provided 
    )
    {
        // calculating index and pages
        ApplyPaging(specParams.PageSize * (specParams.PageIndex -1), specParams.PageSize);      // ApplyPaging(int skip, int take)

        switch (specParams.Sort)                                                // Adds sorting based on the provided criteria
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
