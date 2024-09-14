using System;
using Core.Interfaces;
using System.Linq.Expressions;

namespace Core.Specifications;

public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{
    // private readonly Expression<Func<T, bool>> criteria;
    // public BaseSpecifiaction(Expression<Func<T, bool>> criteria){
    //     this.criteria = criteria;
    // }
    protected BaseSpecification() : this(null) { }                  // Default constructor without criteria
    public Expression<Func<T, bool>>? Criteria => criteria;         // Returns the filtering criteria

    public Expression<Func<T, object>>? OrderBy { get; private set; }            // Stores the ascending order criteria
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }  // Stores the descending order criteria

    public bool IsDistinct { get; private set; }                                // Flag to indicate distinct results

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)    // Adds ascending order condition
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression) // Adds descending order condition
    {
        OrderByDescending = orderByDescExpression;
    }

    protected void ApplyDistinct()                                      // Marks the result set as distinct
    {
        IsDistinct = true;
    }
}

// BaseSpecification
public class BaseSpecification<T, TResult>(Expression<Func<T, bool>> criteria)
    : BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
    protected BaseSpecification() : this(null!) { }
    public Expression<Func<T, TResult>>? Select { get; private set; }

    protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}