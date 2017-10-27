namespace Survey.Business.Services.Implementation
{
    using System.Threading.Tasks;
    using Contracts;
    using Core.Contracts;

    using Entities;
    using Entities.Proxy;
    using Core.Utilities;
    using Data.DataAccess.Repositories;

    public class StudentService : BaseService, IStudentService
    {

        #region Private Members
        private readonly string APIBasePath = System.Web.Configuration.WebConfigurationManager.AppSettings["UserProfileAPIBase"];


        private IAPIService _apiService;
        #endregion

        #region Constructors

        public StudentService(IUnitOfWork unitOfWork, IExceptionReporter exceptionReporter, IAPIService apiService)
            : base(unitOfWork, exceptionReporter)
        {
            _apiService = apiService;
        }

        #endregion

        #region Public Methods
        public async Task<UserDto> GetUserProfile(string userName)
        {
            var apiUrl = UrlExtension.Combine(APIBasePath, "/StudentWithCourseByName/", userName);

            var profile = await _apiService.GetJsonAsync<CUStudentDetail>(apiUrl);

            return AutoMapper.Mapper.Map<UserDto>(profile);

        }

        public async Task<UserDto> GetUserProfile(long studentId)
        {
            var apiUrl = UrlExtension.Combine(APIBasePath, "/StudentWithCourseById/", studentId.ToString());

            var profile = await _apiService.GetJsonAsync<CUStudentDetail>(apiUrl);

            return AutoMapper.Mapper.Map<UserDto>(profile);

        }

        #endregion
    }
}