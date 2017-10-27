namespace Survey.Core.Utilities
{
	using System.Web;
	using Contracts;

	/// <summary>
	/// </summary>
	public class HttpContextBaseWrapper : IHttpContextBaseWrapper
	{
		/// <summary>
		/// </summary>
		private readonly HttpContextBase _context;

		/// <summary>
		/// </summary>
		/// <param name="context"></param>
		public HttpContextBaseWrapper(HttpContextBase context)
		{
			_context = context;
		}

		/// <summary>
		/// </summary>
		public HttpContextBase Context
		{
			get
			{
				return _context;
			}
		}

		/// <summary>
		/// </summary>
		public HttpRequestBase Request
		{
			get
			{
				return _context.Request;
			}
		}

		/// <summary>
		/// </summary>
		public HttpResponseBase Response
		{
			get
			{
				return _context.Response;
			}
		}
	}
}