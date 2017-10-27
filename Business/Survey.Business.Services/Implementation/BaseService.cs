namespace Survey.Business.Services.Implementation
{

    using Core.Contracts;
    using Core.Infrastructure;

    using Castle.Core.Logging;
    using Data.DataAccess.Repositories;
    using System;
    using System.Threading.Tasks;
    using Survey.Business.Services.Contracts;

    /// <summary>
    /// Base class for CRUD operation
    /// </summary>
    public abstract class BaseService : Disposable, IBaseService
    {
        //protected static readonly ILog Logger = LogManager.GetLogger(typeof(BaseService));

        private ILogger logger = NullLogger.Instance;

        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }
        protected readonly IExceptionReporter ExceptionReporter;

        /// <summary>
        /// </summary>
        protected readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="exceptionReporter"></param>
        protected BaseService(IUnitOfWork unitOfWork, IExceptionReporter exceptionReporter)
        {
            UnitOfWork = unitOfWork;
            ExceptionReporter = exceptionReporter;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveAsync()
        {
            try
            {
                await UnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionReporter.Report(ex, Logger);
                return false;
            }
        }

        public bool Save()
        {
            try
            {
                UnitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                ExceptionReporter.Report(ex, Logger);
                return false;
            }
        }

        /// <summary>
        /// </summary>
        protected override void DisposeManaged()
        {
            if (UnitOfWork != null)
            {
                UnitOfWork.Dispose();
            }
        }
    }
}