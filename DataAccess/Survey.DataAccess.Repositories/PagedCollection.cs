namespace Survey.Data.DataAccess.Repositories
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;

	public class PagedCollection<T> : ReadOnlyObservableCollection<T>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="PagedCollection{T}" /> class.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		protected PagedCollection(IEnumerable<T> sequence)
			: this(new ObservableCollection<T>(sequence))
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="PagedCollection{T}" /> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		protected PagedCollection(ObservableCollection<T> collection)
			: this(collection, collection.Count)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="PagedCollection{T}" /> class.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <param name="totalCount">The total count.</param>
		public PagedCollection(IEnumerable<T> sequence, long totalCount)
			: this(new ObservableCollection<T>(sequence), totalCount)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="PagedCollection{T}" /> class.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <param name="totalCount">The total count.</param>
		public PagedCollection(ObservableCollection<T> collection, long totalCount)
			: base(collection)
		{
			TotalCount = totalCount;
		}

		/// <summary>
		///     Gets or sets the total count.
		/// </summary>
		/// <value>
		///     The total count.
		/// </value>
		public long TotalCount { get; protected set; }
	}
}