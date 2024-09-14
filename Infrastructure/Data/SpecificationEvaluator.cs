using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    // This method builds the query based on the specification (filters, sorting, distinct) and returns the filtered result.
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

        return query;  // Returns the query after applying filtering, ordering, and distinctness
    }

    // This method handles projections, which transform entities into different result types (e.g., extracting specific fields).
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

        // If projection was applied, return the projected query, otherwise cast the original query to TResult type.
        return selectQuery ?? query.Cast<TResult>();
    }
}
