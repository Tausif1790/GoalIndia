using System;
using Core.Entities;

namespace Core.Interfaces;

// where => adding constrain -> T has to be a type of BaseEntity
// Generic repository interface constrained by BaseEntity
public interface IGenericRepository<T> where T : BaseEntity      
{
    Task<T?> GetByIdAsync(int id);                               // Fetches an entity by its ID
    Task<IReadOnlyList<T>> ListAllAsync();                       // Fetches a list of all entities
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);          // Fetches a single entity that matches a specification
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);    // Fetches a list of entities that match a specification

    // Supporting projections by returning different result types
    Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec);
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);

    void Add(T entity);                                          // Adds a new entity to the repository
    void Update(T entity);                                       // Updates an existing entity
    void Remove(T entity);                                       // Removes an entity
    Task<bool> SaveAllAsync();                                   // Saves changes to the database
    bool Exists(int id);                                         // Checks if an entity exists by ID
}

