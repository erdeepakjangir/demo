namespace Survey.Core.Utilities
{
    using System.Web;
    using Contracts;

    /// <summary>
    /// </summary>
    public class WebUtility : IWebUtility
	{

		/// <summary>
		/// </summary>
		private readonly HttpRequestBase _request;

		/// <summary>
		/// </summary>
		/// <param name="contextWrapper"></param>
		public WebUtility(IHttpContextBaseWrapper contextWrapper)
		{
			_request = contextWrapper.Request;
		}

		/// <summary>
		/// </summary>
		public string IpAddress
		{
			get
			{
				string ip = _request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				if (string.IsNullOrEmpty(ip))
				{
					ip = _request.ServerVariables["REMOTE_ADDR"];
				}
				return !string.IsNullOrEmpty(ip) ? ((ip.Length > 40) ? ip.Substring(0, 39) : ip) : ip;
			}
		}

		/// <summary>
		/// </summary>
		public string Via
		{
			get
			{
				string via = _request.ServerVariables["HTTP_VIA"];
				return !string.IsNullOrEmpty(via) ? ((via.Length > 250) ? via.Substring(0, 250) : via) : via;
			}
		}

		/// <summary>
		/// </summary>
		public string XForwardedFor
		{
			get
			{
				string xForwardedFor = _request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				return !string.IsNullOrEmpty(xForwardedFor) ? ((xForwardedFor.Length > 250) ? xForwardedFor.Substring(0, 250) : xForwardedFor) : xForwardedFor;
			}
		}
	}
}