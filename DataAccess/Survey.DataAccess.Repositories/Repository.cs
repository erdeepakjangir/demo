namespace Survey.Data.DataAccess.Repositories
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;



    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly SurveyToolDBContext _context;
        private readonly DbSet<T> _dbSet;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repository{T}" /> class.
        /// </summary>
        /// <param name="context">The PS Billing Context.</param>
        public Repository(SurveyToolDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            DbEntityEntry dbEntityEntry = _context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                _dbSet.Add(entity);
            }
        }

        /// <summary>
        ///     Adds the specified entity list.
        /// </summary>
        /// <param name="entity">The entity list.</param>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public void AddAll(List<T> entityList)
        {
            if (entityList == null)
            {
                throw new ArgumentNullException("entityList");
            }
            _dbSet.AddRange(entityList);
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            DbEntityEntry dbEntityEntry = _context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        /// <summary>
        ///     Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException">entity</exception>
        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            DbEntityEntry dbEntityEntry = _context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
            }


        }

        public void RemoveAll(IEnumerable<T> entityList)
        {
            if (entityList == null)
            {
                throw new ArgumentNullException("entity");
            }

            foreach (var entity in entityList)
            {
                DbEntityEntry dbEntityEntry = _context.Entry(entity);
                if (dbEntityEntry.State != EntityState.Deleted)
                {
                    dbEntityEntry.State = EntityState.Deleted;
                }
                else
                {
                    _dbSet.Attach(entity);
                    _dbSet.Remove(entity);
                }
            }
        }

        /// <summary>
        ///     Removes the specified entity.
        /// </summary>
        /// <param name="predicate"></param>
        public void Remove(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> records = _dbSet.Where(predicate);

            foreach (T record in records)
            {
                Remove(record);
            }
        }

        /// <summary>
        ///     Gets the asynchronous.
        /// </summary>
        /// <param name="queryShaper">The query shaper.</param>
        /// <returns></returns>
        public IEnumerable<T> Get(Func<IQueryable<T>, IQueryable<T>> queryShaper)
        {
            IQueryable<T> query = queryShaper(_dbSet);
            return query.ToArray();
        }


        public IQueryable<T> GetQuery(Func<IQueryable<T>, IQueryable<T>> queryShaper)
        {
            return queryShaper(_dbSet);
        }

        /// <summary>
        ///     Gets the asynchronous.
        /// </summary>
        /// <param name="queryShaper">The query shaper.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> queryShaper, CancellationToken cancellationToken)
        {
            IQueryable<T> query = queryShaper(_dbSet);
            return await query.ToArrayAsync(cancellationToken);
        }

        /// <summary>
        ///     Gets the asynchronous.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryShaper">The query shaper.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<TResult> GetAsync<TResult>(Func<IQueryable<T>, TResult> queryShaper, CancellationToken cancellationToken)
        {
            TResult query = queryShaper(_dbSet);
            TaskFactory<TResult> factory = Task<TResult>.Factory;
            return await factory.StartNew(() => query, cancellationToken);
        }

        /// <summary>
        ///     Counts the asynchronous.
        /// </summary>
        /// <param name="queryShaper">The query shaper.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task<int> CountAsync(Func<IQueryable<T>, IQueryable<T>> queryShaper, CancellationToken cancellationToken)
        {
            return queryShaper(_dbSet).CountAsync(cancellationToken);
        }
    }
}