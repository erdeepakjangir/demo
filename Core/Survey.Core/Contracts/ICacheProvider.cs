namespace Survey.Core.Contracts
{
	using Survey.Core.Enums;

	/// <summary>
	/// </summary>
	public interface ICacheProvider
	{
		/// <summary>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="cacheItemType"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		bool AddOrReplace<T>(object key, CacheItemType cacheItemType, T value) where T : class;

		/// <summary>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="cacheItemType"></param>
		/// <returns></returns>
		T Get<T>(object key, CacheItemType cacheItemType) where T : class;

		/// <summary>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="cacheItemType"></param>
		/// <returns></returns>
		bool Remove(object key, CacheItemType cacheItemType);

		/// <summary>
		/// </summary>
		/// <param name="key"></param>
		void Clear(object key);

		/// <summary>
		/// </summary>
		void ClearAll();
	}
}