using Bz.Fott.Registration.Application.Common;
using Bz.Fott.Registration.Domain.Common;
using Bz.Fott.Registration.Infrastructure.Persistence.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bz.Fott.Registration.Infrastructure.Persistence;

internal class ApplicationDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _transaction;
    private readonly IMediator _mediator;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options, 
        IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("Transaction has already started");
        }

        _transaction = await Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
        }
        else
        {
            await SaveChangesAsync();
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            _transaction = null;
        }
    }

    public override int SaveChanges()
    {
        var response = base.SaveChanges();
        DispatchDomainEvents().GetAwaiter().GetResult();
        return response;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var response = await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEvents();
        return response;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Repository<,,>).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    private async Task DispatchDomainEvents()
    {
        var domainEventEntities = ChangeTracker.Entries<IDispatchableDomainEventsEntity>()
            .Select(po => po.Entity)
            .Where(po => po.GetDomainEvents().Any())
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            var events = entity.GetDomainEvents().ToArray();
            entity.ClearDomainEvents();
            foreach (var entityDomainEvent in events)
                await _mediator.Publish(entityDomainEvent);
        }
    }
}
