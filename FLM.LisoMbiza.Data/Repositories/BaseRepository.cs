using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLM.LisoMbiza
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        #region Properties

        protected FLMContext FLMContext { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// </summary>
        /// <param name="archContext"></param>
        public BaseRepository(FLMContext flmContext)
        {
            FLMContext = flmContext;
        }

        #endregion

        #region Implemented Members

        /// <summary>
        /// Creates the entity
        /// </summary>
        /// <param name="entity"></param>
        public void Create(TEntity entity)
        {
            FLMContext.Set<TEntity>().Add(entity);
        }
              
        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            FLMContext.Set<TEntity>().Update(entity);
        }

        /// <summary>
        /// Deletes the entity
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            FLMContext.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            return FLMContext.Set<TEntity>().AsNoTracking();
        }

        /// <summary>
        /// Gets all entities by query or filter
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetByQuery(Expression<Func<TEntity, bool>> expression)
        {
            return FLMContext.Set<TEntity>().Where(expression).AsNoTracking();
        }

        /// <summary>
        /// Creates the entity asynchronously
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task CreateAsync(TEntity entity)
        {
            await FLMContext.Set<TEntity>().AddAsync(entity);
        }

        /// <summary>
        /// Creates the entity list asynchronously
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public async Task CreateListAsync(IEnumerable<TEntity> entityList)
        {
            await FLMContext.Set<TEntity>().AddRangeAsync(entityList);
        }

        /// <summary>
        /// Updates the entity asynchronously
        /// </summary>MO
        /// <param name="entity"></param>
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            FLMContext.Set<TEntity>().Update(entity);
            return await Task.FromResult(true);
        }

        /// <summary>
        /// Deletes the entity asynchronously
        /// </summary>
        /// <param name="entity"></param>
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            FLMContext.Set<TEntity>().Remove(entity);
            return await Task.FromResult(true);
        }

        /// <summary>
        /// Gets all entities asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await FLMContext.Set<TEntity>().AsNoTracking().ToArrayAsync();
        }

        /// <summary>
        /// Gets all entities by query or filter asynchronously
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<TEntity> GetByQueryAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await FLMContext.Set<TEntity>().Where(expression).AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets all by query asynchronously
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllByQueryAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await FLMContext.Set<TEntity>().Where(expression).AsNoTracking().ToArrayAsync();
        }

        #endregion
    }
}
