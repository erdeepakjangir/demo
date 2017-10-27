namespace Survey.Data.DataAccess.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;

	public interface IRepository<T> where T : class
	{
		/// <summary>
		///     Adds the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Add(T entity);
		/// <summary>
		///     Adds the specified entity list.
		/// </summary>

		/// <param name="entity">The entity list.</param>
		void AddAll(List<T> entity);

		/// <summary>
		///     Updates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Update(T entity);

		/// <summary>
		///     Removes the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Remove(T entity);

		void RemoveAll(IEnumerable<T> entityList);

		/// <summary>
		///     Removes the specified predicate.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		void Remove(Expression<Func<T, bool>> predicate);

		/// <summary>
		///     Gets the synchronous. Don't Use it unless required
		/// </summary>
		/// <param name="queryShaper">The query shaper.</param>
		/// <returns></returns>
		IEnumerable<T> Get(Func<IQueryable<T>, IQueryable<T>> queryShaper);

		IQueryable<T> GetQuery(Func<IQueryable<T>, IQueryable<T>> queryShaper);

		/// <summary>
		///     Gets the asynchronous.
		/// </summary>
		/// <param name="queryShaper">The query shaper.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>> queryShaper, CancellationToken cancellationToken);

		/// <summary>
		///     Gets the asynchronous.
		/// </summary>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="queryShaper">The query shaper.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		Task<TResult> GetAsync<TResult>(Func<IQueryable<T>, TResult> queryShaper, CancellationToken cancellationToken);

		/// <summary>
		///     Counts the asynchronous.
		/// </summary>
		/// <param name="queryShaper">The query shaper.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		Task<int> CountAsync(Func<IQueryable<T>, IQueryable<T>> queryShaper, CancellationToken cancellationToken);
	}
}