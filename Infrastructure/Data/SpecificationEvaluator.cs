using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

// The SpecificationEvaluator class, which builds and executes queries based on provided specifications
// for filtering, sorting, distinct results, pagination, and projection:
public class SpecificationEvaluator<T> where T : BaseEntity
{
    // This method generates an IQueryable<T> query based on the specification (ISpecification<T>).
    // It handles filtering, sorting (both ascending and descending), ensuring distinct results, and pagination.
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        // Applies filtering criteria if defined in the specification (e.g., filtering by brand or type)
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);       // Example: x => x.Brand == brand. This query is sent to the database.
        }

        // Applies ascending order if defined in the specification
        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        
        // Applies descending order if defined in the specification
        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        // Ensures distinct results if the specification requires distinct values
        if (spec.IsDistinct) 
        {
            query = query.Distinct();
        }

        if (spec.IsPagingEnabled) 
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        return query;  // Returns the query after applying filtering, ordering, and distinctness
    }

    // This method builds queries that can return projections of different types (TResult), as defined by the ISpecification<T, TResult>.
    // This method allows extracting specific fields or transforming entities into different shapes.
    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query, 
        ISpecification<T, TResult> spec)
    {
        // Applies filtering criteria from the specification (if any)
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria); // Example: x => x.Brand == brand
        }

        // Applies ascending order (if any) from the specification
        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        // Applies descending order (if any) from the specification
        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        // Initializes the result query as an IQueryable<TResult> (which could be a projection of specific fields)
        var selectQuery = query as IQueryable<TResult>; // Casts the query to TResult type (used for projection later)

        // Applies projection if the specification includes a select statement
        if (spec.Select != null)
        {
            selectQuery = query.Select(spec.Select);  // Projection: Transforms the result into a different shape (e.g., selecting only 'Brand' or 'Type' from Product).
        }

        // Ensures distinct results if required by the specification
        if (spec.IsDistinct) 
        {
            selectQuery = selectQuery?.Distinct();
        }

        if (spec.IsPagingEnabled) 
        {
            selectQuery = selectQuery?.Skip(spec.Skip).Take(spec.Take);
        }

        // If projection was applied, return the projected query, otherwise cast the original query to TResult type.
        return selectQuery ?? query.Cast<TResult>();
    }
}


// Projection Explained
// In the second GetQuery method, projection is achieved using the Select expression from the ISpecification<T, TResult> interface.
// Projection in this context refers to selecting a subset of properties from an entity or transforming the entity into a different type,
// rather than returning the entire entity.

// For example
// If you're querying a Product entity but you only need the Name and Price properties, a projection allows you to select only those fields,
// reducing the data load and improving query performance.

// public class ProductNamePriceSpecification : BaseSpecification<Product, string>
// {
//     public ProductNamePriceSpecification()
//     {
//         AddSelect(p => $"{p.Name} - {p.Price}");  // Projects the Name and Price properties into a string
//     }
// }
