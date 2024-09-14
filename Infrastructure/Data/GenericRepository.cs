using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

// we using T instead of product
public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity
{
    // write all method in such a way that applicable of all entities
    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public bool Exists(int id)
    {
        return context.Set<T>().Any(x => x.Id == id);       // our baseEntity has "id" property, therefore we have access of "id" property inside generic repository
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    // Retrieves a single entity based on a specification
    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    // Applies a specification and returns a projected result
    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    // Fetches a list of entities that match a specification
    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {   
        // applying Specification (helper method)
        return await ApplySpecification(spec).ToListAsync();         //So this specification with the way that we've currently set it up, 
    }                                                           //that would effectively filter our list based on the spec and return the list.

    // Add comments
    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(T entity)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }

    // helper method
    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        // calling GetQuery method from "SpecificationEvaluator" class, passing a queriable bases on the type also passing "spec"
        return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
    }

    // Add comments
    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec)
    {
        return SpecificationEvaluator<T>.GetQuery<T, TResult>(context.Set<T>().AsQueryable(), spec);
    }
}