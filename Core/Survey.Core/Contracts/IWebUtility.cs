namespace Survey.Core.Contracts
{
	/// <summary>
	/// </summary>
	public interface IWebUtility
	{
		/// <summary>
		/// </summary>
		string IpAddress { get; }

		/// <summary>
		/// </summary>
		string Via { get; }

		/// <summary>
		/// </summary>
		string XForwardedFor { get; }

	}
}