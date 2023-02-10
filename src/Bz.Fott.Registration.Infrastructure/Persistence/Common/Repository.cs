using Bz.Fott.Registration.Application.Common;
using Bz.Fott.Registration.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bz.Fott.Registration.Infrastructure.Persistence.Common;

internal abstract class Repository<TEntity, TId, TDbContext> : IRepository<TEntity, TId>
    where TEntity : Entity<TId>, IAggregateRoot
    where TDbContext : DbContext, IUnitOfWork
{
    protected readonly TDbContext _dbContext;

    protected Repository(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TId> CreateAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        return entity.Id;
    }

    public async Task DeleteAsync(TId id)
    {
        var entity = await GetAsync(id);
        if (entity is null)
        {
            throw new InvalidOperationException("Cannot delete entity because it does not exist");
        }
        _dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();
        query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return await query.ToListAsync();
    }

    public async Task<TEntity?> GetAsync(TId id, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _dbContext.Set<TEntity>().Where(e => e.Id.Equals(id));
        query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return await query.FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
    }
}
