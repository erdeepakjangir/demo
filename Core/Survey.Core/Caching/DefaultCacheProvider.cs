namespace Survey.Core.Caching
{
	using System;
	using System.Collections;
	using System.Linq;
	using System.Web.Caching;
	using Survey.Core.Contracts;
	using Survey.Core.Enums;

	/// <summary>
	/// </summary>
	public abstract class DefaultCacheProvider : ICacheProvider
	{
		/// <summary>
		/// </summary>
		private const string KeyFormat= "{0}_{1}";

		/// <summary>
		/// </summary>
		protected abstract Cache Cache { get; }

		/// <summary>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="cacheItemType"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool AddOrReplace<T>(object key, CacheItemType cacheItemType, T value) where T : class
		{
			if (value == null)
			{
				return false;
			}
			string formattedKey = FormatKey(key, cacheItemType);
			TimeSpan cacheTimeOut = GetCacheTimeOut();

			if (Cache[formattedKey] != null)
			{
				Remove(key, cacheItemType);
			}

			Cache.Add(formattedKey, value, null, Cache.NoAbsoluteExpiration, cacheTimeOut, CacheItemPriority.Normal, null);

			return true;
		}

		/// <summary>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="cacheItemType"></param>
		/// <returns></returns>
		public T Get<T>(object key, CacheItemType cacheItemType) where T : class
		{
			string formattedKey = FormatKey(key, cacheItemType);

			var value = Cache.Get(formattedKey) as T;

			return value;
		}

		/// <summary>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="cacheItemType"></param>
		/// <returns></returns>
		public bool Remove(object key, CacheItemType cacheItemType)
		{
			string formattedKey = FormatKey(key, cacheItemType);

			object removedItem = Cache.Remove(formattedKey);

			if (removedItem != null)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// </summary>
		/// <param name="key"></param>
		public void Clear(object key)
		{
			string partialKey = string.Format(KeyFormat, key, string.Empty);
			foreach (DictionaryEntry entry in Cache.Cast<DictionaryEntry>().Where(e => Convert.ToString(e.Key).Contains(partialKey)))
			{
				Cache.Remove(entry.Key.ToString());
			}
		}

		/// <summary>
		/// </summary>
		public void ClearAll()
		{
			foreach (DictionaryEntry entry in Cache.Cast<DictionaryEntry>())
			{
				Cache.Remove(entry.Key.ToString());
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="key"></param>
		/// <param name="cacheItemType"></param>
		/// <returns></returns>
		private static string FormatKey(object key, CacheItemType cacheItemType)
		{
			return string.Format(KeyFormat, key, cacheItemType);
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		protected abstract TimeSpan GetCacheTimeOut();
	}
}