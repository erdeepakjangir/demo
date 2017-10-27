using Survey.Business.Entities;
using Survey.Business.Entities.Proxy;
using Survey.Business.Entities.Enums;
using Survey.Business.Services.Contracts;
using Survey.Core.Contracts;

using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Survey.Core.Enums;
using Survey.Core.Utilities;
using Survey.Data.DataAccess.Repositories;

namespace Survey.Business.Services.Implementation
{
    /// <summary>
    /// Class to get the master data from CU web Service
    /// This class will Cache data
    /// </summary>
    public class QualificationService : BaseService, IQualificationService
    {
        #region Private Members
        const string QualificationKey = "QualificationKey";
        const string SittingKey = "SittingKey";
        const string FacultyKey = "FacultyKey";
        private readonly string APIBasePath = System.Web.Configuration.WebConfigurationManager.AppSettings["MasterDataAPIBase"];

        private readonly ICacheProvider _cacheService;
        private readonly IAPIService _apiService;
        #endregion

        #region Constructors

        public QualificationService(IUnitOfWork unitOfWork, IExceptionReporter exceptionReporter, ICacheProvider cacheService, IAPIService apiService)
            : base(unitOfWork, exceptionReporter)
        {


            _cacheService = cacheService;
            _apiService = apiService;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get Year List
        /// </summary>
        /// <returns></returns>
        public async Task<List<int>> GetYears()
        {

            List<int> years = new List<int>();

            for (int index = DateTime.Today.Year; index > DateTime.Today.Year - 10; index--)
            {
                years.Add(index);
            }
            return years;

        }

        /// <summary>
        /// Get Qualification List 
        /// </summary>
        /// <returns></returns>
        public async Task<QualificationDataListDto> GetQualifications()
        {
            QualificationDataListDto qualificationList;

            qualificationList = _cacheService.Get<QualificationDataListDto>(QualificationKey, CacheItemType.QualificationMaster);
            if (qualificationList == null)
            {
                //get from Master
                var apiUrl = UrlExtension.Combine(APIBasePath, "/QualificationTypes/");

                var qualifications = await _apiService.GetJsonAsync<List<CUQualificationType>>(apiUrl);

                qualificationList = new QualificationDataListDto();
                //sorting of Course / subjects / Grade and hide by VisibleOle
                foreach (var qual in qualifications.Where(q => q.VisibleOle == true).OrderBy(q => q.Description))
                {
                    var newqualification = AutoMapper.Mapper.Map<QualificationDataDto>(qual);

                    apiUrl = UrlExtension.Combine(APIBasePath, "/QualificationTypeSubjects/", qual.QualificationTypeCode);
                    var subjects = await _apiService.GetJsonAsync<List<CUSubject>>(apiUrl);
                    newqualification.Subjects = AutoMapper.Mapper.Map<List<SubjectDto>>(subjects.Where(q => q.VisibleOle == true)).OrderBy(s => s.SubjectTitle).ToList();

                    apiUrl = UrlExtension.Combine(APIBasePath, "/QualificationTypeResults/", qual.QualificationTypeCode);
                    var results = await _apiService.GetJsonAsync<List<CUResult>>(apiUrl);
                    newqualification.Results = AutoMapper.Mapper.Map<List<ResultDto>>(results.Where(q => q.VisibleOle == true)).OrderBy(r => r.Result).ToList();

                    qualificationList.Add(newqualification);
                }

                _cacheService.AddOrReplace(QualificationKey, Core.Enums.CacheItemType.QualificationMaster, qualificationList);
            }

            return qualificationList;
        }

        /// <summary>
        /// Get subjects for Qualification
        /// </summary>
        /// <param name="qualificationCode"></param>
        /// <returns></returns>
        public async Task<List<SubjectDto>> GetSubjects(string qualificationCode)
        {
            var qualificationList = await this.GetQualifications();
            return qualificationList.FirstOrDefault(c => c.Code == qualificationCode).Subjects;

        }

        /// <summary>
        /// Get Result for Qualification
        /// </summary>
        /// <param name="qualificationCode"></param>
        /// <returns></returns>
        public async Task<List<ResultDto>> GetResults(string qualificationCode)
        {
            var qualificationList = await this.GetQualifications();
            return qualificationList.FirstOrDefault(c => c.Code == qualificationCode).Results;

        }

        /// <summary>
        /// Get qualification  Name, Subject Name, Result, Sitting Name by code
        /// </summary>
        /// <param name="qualificationCode"></param>
        /// <param name="subjectCode"></param>
        /// <param name="result"></param>
        /// <param name="sittingCode"></param>
        /// <returns></returns>
        public async Task<string[]> GetNamesByCode(string qualificationCode, string subjectCode, string result, char sittingCode)
        {
            string[] names = new string[4];

            var courses = await this.GetQualifications();
            var sittings = await this.GetSittings();
            names[0] = courses.FirstOrDefault(c => c.Code == qualificationCode).Title;
            names[1] = courses.FirstOrDefault(c => c.Code == qualificationCode).Subjects.FirstOrDefault(s => s.SubjectCode == subjectCode).SubjectTitle;
            names[2] = courses.FirstOrDefault(c => c.Code == qualificationCode).Results.FirstOrDefault(r => r.Result == result).Result;
            names[3] = sittings.FirstOrDefault(s => s.SittingCode == sittingCode).SittingTitle;

            return names;

        }

        /// <summary>
        /// Get GetSittings for Qualification
        /// </summary>
        /// <returns></returns>
        public async Task<List<SittingDto>> GetSittings()
        {
            List<SittingDto> sitings = _cacheService.Get<List<SittingDto>>(SittingKey, Core.Enums.CacheItemType.SittingMaster);

            if (sitings == null)
            {
                sitings = new List<SittingDto> { new SittingDto { SittingCode = 'S', SittingTitle = "Summer" }, new SittingDto { SittingCode = 'W', SittingTitle = "Winter" } };

                //Add to cache
                _cacheService.AddOrReplace(QualificationKey, Core.Enums.CacheItemType.SittingMaster, sitings);
            }

            return sitings;
        }


        /// <summary>
        ///  Get Qualification Statuses
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetStatuses()
        {
            return Enum.GetNames(typeof(Core.Enums.QualificationStatus)).ToList();
        }

        /// <summary>
        /// Get List of faculties, Name and Code
        /// </summary>
        /// <returns></returns>
        public async Task<List<FacultyDto>> GetFaculties()
        {
            List<FacultyDto> facultyList;

            facultyList = _cacheService.Get<List<FacultyDto>>(FacultyKey, Core.Enums.CacheItemType.FacultyMaster);
            if (facultyList == null)
            {
                var apiUrl = UrlExtension.Combine(APIBasePath, "/AdmissionsFaculties");

                var faculties = await _apiService.GetJsonAsync<List<CUFaculty>>(apiUrl);
                // Sort 
                facultyList = AutoMapper.Mapper.Map<List<FacultyDto>>(faculties).OrderBy(f => f.FacultyName).ToList();

                _cacheService.AddOrReplace<List<FacultyDto>>(FacultyKey, Core.Enums.CacheItemType.FacultyMaster, facultyList);
            }

            return facultyList;
        }

        /// <summary>
        /// Get Faculty code from its Name
        /// </summary>
        /// <param name="facultyName"></param>
        /// <returns></returns>
        public async Task<string> GetFacultyCode(string facultyName)
        {
            var facultyList = await GetFaculties();
            // Matching facultyCode for Dev purpose.
            return facultyList.FirstOrDefault(f => f.FacultyName == facultyName || f.FacultyCode == facultyName).FacultyCode;
        }

        #endregion
    }

}
