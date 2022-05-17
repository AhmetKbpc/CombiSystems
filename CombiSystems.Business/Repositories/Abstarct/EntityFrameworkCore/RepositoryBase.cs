using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CombiSystems.Core.Entities.Abstracts;
using CombiSystems.Data.EntityFramework;

namespace CombiSystems.Business.Repositories.Abstracts.EntityFrameworkCore;

public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
    where TKey : IEquatable<TKey>
    where TEntity : BaseEntity<TKey>
{
    protected readonly MyContext _context;
    protected readonly DbSet<TEntity> _table;

    protected RepositoryBase(MyContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }
    public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null)
    {
        return predicate == null ? _table : _table.Where(predicate);
    }
    public virtual TEntity GetById(TKey id)
    {
        return _table.Find(id);
    }

    public virtual TKey Insert(TEntity entity, bool isSaveLater = false)
    {
        _table.Add(entity);
        if (!isSaveLater)
            this.Save();
        return entity.Id;
    }

    public virtual int Update(TEntity entity, bool isSaveLater = false)
    {
        _table.Update(entity);
        if (!isSaveLater)
            return this.Save();
        return 0;
    }

    public virtual int Delete(TEntity entity, bool isSaveLater = false)
    {
        _table.Remove(entity);
        if (!isSaveLater)
            return this.Save();
        return 0;
    }

    public virtual int Save()
    {
        return _context.SaveChanges();
    }
}