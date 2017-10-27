namespace Survey.Data.DataAccess.Repositories
{
    using System.Data;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Survey.Core.Infrastructure;

    using System.Data.Entity.Validation;
    using System.Linq;
    using Survey.Data.DataAccess.Entities;
    using System;

    public sealed class UnitOfWork : Disposable, IUnitOfWork
    {
        private readonly SurveyToolDBContext _context;
        private readonly bool _lazyLoadingEnabled;
        private IRepository<SurveyMaster> _surveyRepository;
        private IRepository<SurveyDetail> _surveyDetailRepository;
        private IRepository<QuestionBank> _questionbankRepository;
        /// <summary>
        ///     Initializes a new instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UnitOfWork(SurveyToolDBContext context)
        {
            _context = context;
#if DEBUG
            _context.Database.Log = s => Debug.WriteLine(s);
#endif
            _lazyLoadingEnabled = true; //Default
        }

        /// <summary>
        ///     Commits this instance.
        /// </summary>
        public Task CommitAsync()
        {
            try
            {
                return _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task CommitAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        ///     Commits this instance.
        /// </summary>         

        public void Commit()
        {
            _context.SaveChanges();
        }


        /// <summary>
        /// </summary>
        /// <param name="isolationLevel"></param>
        public void SetIsolationLevel(IsolationLevel isolationLevel)
        {
            ObjectContext objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            objectContext.ContextOptions.LazyLoadingEnabled = _lazyLoadingEnabled;
            objectContext.ContextOptions.ProxyCreationEnabled = false;
            switch (isolationLevel)
            {
                case IsolationLevel.ReadUncommitted:
                    objectContext.ExecuteStoreCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                    break;

                case IsolationLevel.ReadCommitted:
                    objectContext.ExecuteStoreCommand("SET TRANSACTION ISOLATION LEVEL READ COMMITTED;");
                    break;

                case IsolationLevel.Chaos:
                    objectContext.ExecuteStoreCommand("SET TRANSACTION ISOLATION LEVEL CHAOS;");
                    break;

                case IsolationLevel.RepeatableRead:
                    objectContext.ExecuteStoreCommand("SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;");
                    break;

                case IsolationLevel.Serializable:
                    objectContext.ExecuteStoreCommand("SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;");
                    break;

                case IsolationLevel.Snapshot:
                    objectContext.ExecuteStoreCommand("SET TRANSACTION ISOLATION LEVEL SNAPSHOT;");
                    break;

                case IsolationLevel.Unspecified:
                    objectContext.ExecuteStoreCommand("SET TRANSACTION ISOLATION LEVEL UNSPECIFIED;");
                    break;

                default:
                    objectContext.ExecuteStoreCommand("SET TRANSACTION ISOLATION LEVEL READ COMMITTED;");
                    break;
            }
        }

        /// <summary>
        /// </summary>
        public bool LazyLoadingEnabled
        {
            get { return ((IObjectContextAdapter)_context).ObjectContext.ContextOptions.LazyLoadingEnabled; }
            set { ((IObjectContextAdapter)_context).ObjectContext.ContextOptions.LazyLoadingEnabled = value; }
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources
        /// </summary>
        protected override void DisposeManaged()
        {
            if (_context == null)
            {
                return;
            }
            _context.Dispose();
        }

        public IRepository<SurveyMaster> SurveyRepository
        {
            get { return _surveyRepository ?? (_surveyRepository = new Repository<SurveyMaster>(_context)); }
        }

        public IRepository<QuestionBank> QuestionbankRepository
        {
            get
            {
                return _questionbankRepository ?? (_questionbankRepository = new Repository<QuestionBank>(_context));
            }
        }
    }
}