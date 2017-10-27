namespace Survey.Core.Contracts
{
	using System.Web;

	/// <summary>
	/// </summary>
	public interface IHttpContextBaseWrapper
	{
		/// <summary>
		/// </summary>
		HttpContextBase Context { get; }

		/// <summary>
		/// </summary>
		HttpRequestBase Request { get; }

		/// <summary>
		/// </summary>
		HttpResponseBase Response { get; }
	}
}