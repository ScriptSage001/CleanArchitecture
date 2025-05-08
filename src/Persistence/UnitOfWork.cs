using Application.Abstractions.Authentication;
using Application.Abstractions.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Primitives;

namespace Persistence;

public sealed class UnitOfWork(
    ApplicationDbContext appDbContext,
    IUserContext userContext,
    IMediator mediator)
    : IUnitOfWork
{
    private readonly ApplicationDbContext _appDbContext = appDbContext;
    private readonly IUserContext _userContext = userContext;
    private readonly IMediator _mediator = mediator;

    #region Public Methods

    /// <summary>
    /// Custom SaveChanges method on top of the SaveChanges of EFCore
    /// Handles Update of Auditable Entites before saving changes to DB
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        HandleSoftDelete();
        var result =  await _appDbContext.SaveChangesAsync(cancellationToken);
        await DispatchDomainEvents(cancellationToken);
        return result;
    }

    /// <summary>
    /// Asynchronously begins a new database transaction.
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        await _appDbContext
            .Database
            .BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Disposes the context and releases all resources used by the UnitOfWork.
    /// </summary>
    public void Dispose()
    {
        _appDbContext.Dispose();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// To set Auditable Properties of the Auditable Entities
    /// Using ChangeTracker of EF Core
    /// </summary>
    private void UpdateAuditableEntities()
    {
        var currentUser = _userContext.GetUserName();
        var entities = _appDbContext.ChangeTracker.Entries<IAuditable>();

        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                entity
                    .Property(x => x.CreatedOn)
                    .CurrentValue = DateTimeOffset.Now;

                entity
                    .Property(x => x.CreatedBy)
                    .CurrentValue = currentUser!;
            }

            entity
                .Property(x => x.UpdatedOn)
                .CurrentValue = DateTimeOffset.Now;

            entity
                .Property(x => x.UpdatedBy)
                .CurrentValue = currentUser!;
        }
    }

    /// <summary>
    /// To set SoftDelete Property of the SoftDeletable Entities
    /// </summary>
    private void HandleSoftDelete()
    {
        var entities = _appDbContext.ChangeTracker.Entries<Entity>();

        foreach (var entity in entities)
        {
            switch (entity.State)
            {
                case EntityState.Deleted:
                    entity.State = EntityState.Modified;
                    entity
                        .Property(x => x.IsDeleted)
                        .CurrentValue = true;
                    break;

                case EntityState.Added:
                    entity
                        .Property(x => x.IsDeleted)
                        .CurrentValue = false;
                    break;

                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Modified:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    /// <summary>
    /// Dispatches Domain Events to the Messaging System
    /// </summary>
    private async Task DispatchDomainEvents(CancellationToken cancellationToken = default)
    {
        var domainEvents = _appDbContext
                                                .ChangeTracker
                                                .Entries<AggregateRoot>()
                                                .Select(entry => entry.Entity)
                                                .SelectMany(entity =>
                                                {
                                                    var domainEvents = entity.GetDomainEvents();
                                                    entity.ClearDomainEvents();
                                                    return domainEvents;
                                                })
                                                .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }

    #endregion
}