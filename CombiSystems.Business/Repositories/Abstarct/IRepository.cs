using System.Linq.Expressions;
using CombiSystems.Core.Entities.Abstracts;

namespace CombiSystems.Business.Repositories.Abstracts;

public interface IRepository<TEntity, TKey>
    where TKey : IEquatable<TKey>
    where TEntity : BaseEntity<TKey>
{
    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null);
    //Task<IQueryable<TEntity>> GetAsnyc(Expression<Func<TEntity, bool>> predicate = null);
    TEntity GetById(TKey id);
    TKey Insert(TEntity entity, bool isSaveLater = false);
    int Update(TEntity entity, bool isSaveLater = false);
    int Delete(TEntity entity, bool isSaveLater = false);
    int Save();
}