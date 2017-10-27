namespace Survey.Data.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public static class RepositoryExtensions
    {
        /// <summary>
        ///     Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="queryShaper">The query shaper.</param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> GetAsync<T>(this IRepository<T> repository, Func<IQueryable<T>, IQueryable<T>> queryShaper) where T : class
        {
            return repository.GetAsync(queryShaper, CancellationToken.None);
        }

        /// <summary>
        ///     Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="queryShaper">The query shaper.</param>
        /// <returns></returns>
        public static Task<TResult> GetAsync<T, TResult>(this IRepository<T> repository, Func<IQueryable<T>, TResult> queryShaper) where T : class
        {
            return repository.GetAsync(queryShaper, CancellationToken.None);
        }

        /// <summary>
        ///     Gets all asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> GetAllAsync<T>(this IRepository<T> repository) where T : class
        {
            return repository.GetAsync(q => q, CancellationToken.None);
        }

        /// <summary>
        ///     Gets all asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> GetAllAsync<T>(this IRepository<T> repository, CancellationToken cancellationToken) where T : class
        {
            return repository.GetAsync(q => q, cancellationToken);
        }

        /// <summary>
        ///     Finds the by asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> FindByAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate) where T : class
        {
            return repository.GetAsync(q => q.Where(predicate), CancellationToken.None);
        }

        /// <summary>
        ///     Finds the by asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> FindByAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) where T : class
        {
            return repository.GetAsync(q => q.Where(predicate), cancellationToken);
        }

        /// <summary>
        ///     Gets the single asynchronous. Don't Use it unless required
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static T GetSingle<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate) where T : class
        {
            return repository.Get(q => q.Where(predicate)).SingleOrDefault();
        }

        /// <summary>
        ///     Gets the single asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static Task<T> GetSingleAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate) where T : class
        {
            return repository.GetSingleAsync(predicate, CancellationToken.None);
        }

        /// <summary>
        ///     Gets the single asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<T> GetSingleAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken) where T : class
        {
            IEnumerable<T> items = await repository.GetAsync(q => q.Where(predicate), cancellationToken);
            return items.SingleOrDefault();
        }

        /// <summary>
        ///     Paginates the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static Task<PagedCollection<T>> PaginateAsync<T>(this IRepository<T> repository, int pageIndex, int pageSize) where T : class
        {
            return repository.PaginateAsync(pageIndex, pageSize, CancellationToken.None);
        }

        /// <summary>
        ///     Paginates the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<PagedCollection<T>> PaginateAsync<T>(this IRepository<T> repository, int pageIndex, int pageSize, CancellationToken cancellationToken) where T : class
        {
            var groups = await repository.GetAsync(q =>
            {
                int startIndex = pageIndex * pageSize;
                return q.Skip(startIndex).Take(pageSize).GroupBy(g => new
                {
                    Total = q.Count()
                });
            },
                cancellationToken);

            var result = groups.FirstOrDefault();

            if (result == null)
            {
                return new PagedCollection<T>(Enumerable.Empty<T>(), 0L);
            }

            return new PagedCollection<T>(result, result.Key.Total);
        }

        /// <summary>
        ///     Paginates the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="queryShaper">The query shaper.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static Task<PagedCollection<T>> PaginateAsync<T>(this IRepository<T> repository, Func<IQueryable<T>, IQueryable<T>> queryShaper, int pageIndex, int pageSize) where T : class
        {
            return repository.PaginateAsync(queryShaper, pageIndex, pageSize, CancellationToken.None);
        }

        /// <summary>
        ///     Paginates the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="queryShaper">The query shaper.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<PagedCollection<T>> PaginateAsync<T>(this IRepository<T> repository, Func<IQueryable<T>, IQueryable<T>> queryShaper, int pageIndex, int pageSize, CancellationToken cancellationToken) where T : class
        {
            var groups = await repository.GetAsync(q =>
            {
                IQueryable<T> query = queryShaper(q);
                int startIndex = pageIndex * pageSize;
                return query.Skip(startIndex).Take(pageSize).GroupBy(g => new
                {
                    Total = query.Count()
                });
            },
                cancellationToken);

            var result = groups.FirstOrDefault();

            if (result == null)
            {
                return new PagedCollection<T>(Enumerable.Empty<T>(), 0L);
            }

            return new PagedCollection<T>(result, result.Key.Total);
        }

        /// <summary>
        /// Add In clause with Or operator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="query"></param>
        /// <param name="selector"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIn<T, TValue>(this IQueryable<T> query, Expression<Func<T, TValue>> selector, IEnumerable<TValue> collection)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (collection == null) throw new ArgumentNullException("collection");
            ParameterExpression p = selector.Parameters.Single();

            if (!collection.Any()) return query;

            IEnumerable<Expression> equals = collection.Select(value =>
              (Expression)Expression.Equal(selector.Body,
               Expression.Constant(value, typeof(TValue))));

            Expression body = equals.Aggregate((accumulate, equal) =>
            Expression.Or(accumulate, equal));

            return query.Where(Expression.Lambda<Func<T, bool>>(body, p));
        }

    }
}