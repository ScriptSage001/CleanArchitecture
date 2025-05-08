using System.Linq.Expressions;
using SharedKernel.Primitives;

namespace SharedKernel.Abstractions;

/// <summary>
/// Generic repository interface for CRUD operations.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity> where TEntity : Entity
{
    Task<IQueryable<TEntity>> Query(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken);
    Task Delete(TEntity entity, CancellationToken cancellationToken);
}