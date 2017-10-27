namespace Survey.Core.Caching
{
	using System;
	using System.Configuration;
	using System.Web;
	using System.Web.Caching;

	/// <summary>
	/// </summary>
	public class ServiceCacheProvider : DefaultCacheProvider
	{
		/// <summary>
		/// </summary>
		private const string CacheTimeoutKey = "CacheTimeOut";

		/// <summary>
		/// </summary>
		protected override Cache Cache
		{
			get { return HttpRuntime.Cache; }
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		protected override TimeSpan GetCacheTimeOut()
		{
			TimeSpan cacheTimeOut;
			string cacheTimeOutString = ConfigurationManager.AppSettings[CacheTimeoutKey];
			if (cacheTimeOutString != null && TimeSpan.TryParse(cacheTimeOutString, out cacheTimeOut))
			{
				return cacheTimeOut;
			}
			return TimeSpan.MinValue;
		}
	}
}