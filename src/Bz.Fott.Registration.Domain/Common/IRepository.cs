using System.Linq.Expressions;

namespace Bz.Fott.Registration.Domain.Common;

public interface IRepository<TEntity, TId>
    where TEntity : IAggregateRoot
{
    Task<TId> CreateAsync(TEntity entity);

    Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity?> GetAsync(TId id, params Expression<Func<TEntity, object>>[] includes);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TId id);
}
