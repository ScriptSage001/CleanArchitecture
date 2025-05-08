using System.Linq.Expressions;
using SharedKernel.Abstractions;
using SharedKernel.Primitives;

namespace Persistence.Repositories;

/// <summary>
/// Base repository class that implements the IRepository interface.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseRepository<T> : IRepository<T> where T : Entity
{
    private readonly ApplicationDbContext _context;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
    /// </summary>
    /// <param name="context"></param>
    protected BaseRepository(ApplicationDbContext context)
        => _context = context;
    
    /// <summary>
    /// Queries the database for a single entity that matches the given predicate.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IQueryable<T>> Query(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await Task
                        .Run(() => _context
                                        .Set<T>()
                                        .Where(predicate), cancellationToken)
                        .ConfigureAwait(false);
    }

    /// <summary>
    /// Adds a new entity to the database.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<T> Add(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    /// <summary>
    /// Updates an existing entity in the database.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<T> Update(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    /// <summary>
    /// Deletes an entity from the database.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    public async Task Delete(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Remove(entity);
    }
}