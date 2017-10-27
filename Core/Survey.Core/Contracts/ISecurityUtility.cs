namespace Survey.Core.Contracts
{
	using Survey.Core.Enums;

	/// <summary>
	/// </summary>
	public interface ISecurityUtility
	{
		/// <summary>
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		string GetHash(string input);

		/// <summary>
		/// </summary>
		/// <param name="input"></param>
		/// <param name="salt"></param>
		/// <param name="hashAlgorithm"></param>
		/// <param name="iterations"></param>
		/// <returns></returns>
		string GetHash(string input, string salt, byte hashAlgorithm = (byte)EncryptionScheme.SHA512, int iterations = 1);

		/// <summary>
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		string GetBase64EncodedSalt(int bytes = 64);
	}
}