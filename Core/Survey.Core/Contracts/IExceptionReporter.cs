namespace Survey.Core.Contracts
{
    using System;
  
    using Castle.Core.Logging;

    /// <summary>
    /// </summary>
    public interface IExceptionReporter
    {
        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        void Report(Exception ex);

        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="logger"></param>
        void Report(Exception ex, ILogger logger);

        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="details"></param>
        void Report(Exception ex, string details);

        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="details"></param>
        /// <param name="logger"></param>
        void Report(Exception ex, string details, ILogger logger);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        string GenerateErrorText(string details);

    }

}