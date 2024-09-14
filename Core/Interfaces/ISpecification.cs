using System;
using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T> 
{
    Expression<Func<T, bool>>? Criteria { get; }                 // Filter condition for querying entities
    Expression<Func<T, object>>? OrderBy { get; }                // we dont know the result so using object
    Expression<Func<T, object>>? OrderByDescending { get; }      // Ordering in descending order
    bool IsDistinct { get; }                                     // Ensures distinct results if true
}

// Specification that returns different result types (projections)
// Enhance ISpecification not just only takes T parameter but also returns different types
public interface ISpecification<T, TResult> : ISpecification<T>  
{
    Expression<Func<T, TResult>>? Select { get; }                // Defines the fields to project in the result set
}
