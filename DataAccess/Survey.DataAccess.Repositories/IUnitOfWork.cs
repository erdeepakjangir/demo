namespace Survey.Data.DataAccess.Repositories
{
    using Survey.Data.DataAccess.Entities;
    using Entities;
    using System;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;


    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Gets or sets a value indicating whether [lazy loading enabled].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [lazy loading enabled]; otherwise, <c>false</c>.
        /// </value>
        bool LazyLoadingEnabled { get; set; }

        /// <summary>
        /// </summary>
        IRepository<SurveyMaster> SurveyRepository { get; }

         //Added on 26/10/2017
         IRepository<QuestionBank> QuestionbankRepository { get; }


        /// <summary>
        ///     Sets the isolation level.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        void SetIsolationLevel(IsolationLevel isolationLevel);

        /// <summary>
        ///     Commits the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();

        /// <summary>
        /// Commits
        /// </summary>
        void Commit();

        /// <summary>
        ///     Commits the asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CommitAsync(CancellationToken cancellationToken);
    }
}