using System;
using System.Linq.Expressions;

namespace Core.Interfaces;

// The ISpecification<T> interface defines a set of conditions and operations that help in filtering,
// sorting, and paginating query results for a particular entity type T. This pattern is useful when
// you need to apply different kinds of logic (filtering, ordering, pagination) in a clean, reusable way.
public interface ISpecification<T> 
{
    Expression<Func<T, bool>>? Criteria { get; }                 // Filter condition for querying entities
    Expression<Func<T, object>>? OrderBy { get; }                // we dont know the result so using object
    Expression<Func<T, object>>? OrderByDescending { get; }      // Ordering in descending order
    bool IsDistinct { get; }                                     // Ensures distinct results if true
    int Take { get; }                                           // for pagination
    int Skip { get; }                                           // for pagination
    bool IsPagingEnabled { get; }                               // for pagination
    IQueryable<T> ApplyCriteria(IQueryable<T> query);           // Applies filtering criteria to a query
}

// This interface extends ISpecification<T> and introduces the ability to return projections (custom result sets).
// It is particularly useful when you want to retrieve a subset of fields from an entity (rather than the entire entity).

// Projection in Specifications
// In the ISpecification<T, TResult> interface, the projection is defined by the Select property, which allows you to shape the result set into a custom format (TResult).
public interface ISpecification<T, TResult> : ISpecification<T>  
{
    Expression<Func<T, TResult>>? Select { get; }                // Defines the fields to project in the result set
}

// Select: Specifies a projection that transforms the result set into a custom shape (e.g., selecting specific fields rather than entire entities).
// This is useful when you need different projections of an entity, especially for API responses where only certain fields are required.