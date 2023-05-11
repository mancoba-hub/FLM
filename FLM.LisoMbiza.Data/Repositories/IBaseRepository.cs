using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace FLM.LisoMbiza
{
    public interface IBaseRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();

        Task<IEnumerable<TEntity>> GetAllAsync();

        IQueryable<TEntity> GetByQuery(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> GetByQueryAsync(Expression<Func<TEntity, bool>> expression);

        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task CreateAsync(TEntity entity);

        Task CreateListAsync(IEnumerable<TEntity> entityList);

        Task<bool> DeleteAsync(TEntity entity);

        Task<bool> UpdateAsync(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllByQueryAsync(Expression<Func<TEntity, bool>> expression);
    }
}
