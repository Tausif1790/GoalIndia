using Core.Entities;

namespace Core.Specifications;

// This specification class extracts a distinct list of product brands.
public class BrandListSpecification : BaseSpecification<Product, string>
{
    public BrandListSpecification()
    {
        AddSelect(x => x.Brand);  // Projects only the 'Brand' field from the Product entity (projection).
        ApplyDistinct();          // Ensures that only distinct brands are returned (no duplicates).
    }
}
