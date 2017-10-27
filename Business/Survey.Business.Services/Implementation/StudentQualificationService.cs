using Survey.Business.Services.Contracts;
using Survey.Core.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Survey.Business.Entities;

using Survey.Core.Utilities;
using Survey.Business.Entities.Proxy;
using AutoMapper;
using Survey.Core.Enums;

using System.Globalization;
using System.Net.Http;
using System.Web.Mvc;
using System.IO;
using Survey.Data.DataAccess.Repositories;

namespace Survey.Business.Services.Implementation
{
    public class StudentQualificationService : BaseService, IStudentQualificationService
    {
        #region Private Members
        private IStudentService _userService;
        private IAPIService _apiService;
        private readonly string WebApiBasePath = System.Web.Configuration.WebConfigurationManager.AppSettings["WebAPIBase"];
        private readonly string QualificationAPIPath = System.Web.Configuration.WebConfigurationManager.AppSettings["QualificationAPIBase"];
        private readonly string DocumentAPIPath = System.Web.Configuration.WebConfigurationManager.AppSettings["DocumentAPIBase"];


        #endregion

        #region Constructors

        public StudentQualificationService(IUnitOfWork unitOfWork, IExceptionReporter exceptionReporter, IStudentService userService, IAPIService apiService)
         
            : base(unitOfWork, exceptionReporter)
        {
            _userService = userService;
            _apiService = apiService;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public async Task<List<QualificationDto>> GetExistingQualificationsByStudent(long studentId)
        {
            Logger.Info("GetExistingQualificationsByStudent StudentId#:" + studentId);

            //Call Qualification Service
            var apiUrl = UrlExtension.Combine(QualificationAPIPath, "/Qualifications/", studentId.ToString());

            var studentQualifications = Mapper.Map<List<QualificationDto>>(await _apiService.GetJsonAsync<List<CUStudentQualification>>(apiUrl));

            return studentQualifications;
        }

        /// <summary>
        /// GetQualifications By Student Id, Status
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public async Task<List<QualificationDto>> GetQualificationsByStudent(QualificationSearchDto searchCriteria)
        {
            Logger.Info("Search Criteria StudentId#:" + searchCriteria.StudentId);
            return await GetQualifications(searchCriteria);
        }
        /// <summary>
        /// GetQualifications By Faculty - staff
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public async Task<List<QualificationDto>> GetQualificationsByFaculty(QualificationSearchDto searchCriteria)
        {
            Logger.Info("Search Criteria FacultyCode#:" + searchCriteria.FacultyCode[0]);

            return await GetQualifications(searchCriteria);
        }

        /// <summary>
        /// Get Qualifications By Search -  Planner
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public async Task<List<QualificationDto>> GetQualificationsBySearch(QualificationSearchDto searchCriteria)
        {
            Logger.Info("Search Criteria :");

            return await GetQualifications(searchCriteria);
        }

        /// <summary>
        /// Get the report summary
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public async Task<ReportSummaryDto> GetReportBySearch(QualificationSearchDto searchCriteria)
        {
            Logger.Info("Report Search Criteria :");

            var apiUrl = UrlExtension.Combine(WebApiBasePath, "/qualification/report");

            return (await _apiService.PostJsonAsync<QualificationSearchDto, ReportSummaryDto>(apiUrl, searchCriteria));
        }



        /// <summary>
        /// Add Qualification
        /// </summary>
        /// <param name="qualification"></param>
        /// <returns></returns>
        public async Task<long> AddQualification(QualificationDto qualification)
        {
            Logger.Info("AddQualification :" + qualification.QualificationTypeCode + " " + qualification.StudentId.ToString());

            var apiUrl = UrlExtension.Combine(WebApiBasePath, "/qualification/insert");

            var newQualification = Mapper.Map<Qualification>(qualification);
            //created by /Created Date
            newQualification.Status = (int)QualificationStatus.Pending;
            newQualification.CreatedDate = DateTime.Today;
            newQualification.ActionBy = qualification.ActionUserName;

            var newQualificationId = await _apiService.PostJsonAsync<Qualification, long>(apiUrl, newQualification);

            return newQualificationId;
        }

        /// <summary>
        /// Upload Qualification to CU DB
        /// </summary>
        /// <param name="qualification"></param>
        /// <returns></returns>
        public async Task<bool> UploadQualification(QualificationDto qualification)
        {
            //1. upload Qualification to CU

            Logger.Info("UploadQualification :" + qualification.QualificationTypeCode + " " + qualification.StudentId.ToString());

            var apiCUUrl = UrlExtension.Combine(QualificationAPIPath, "/Qualifications/");

            var uploadQualification = Mapper.Map<CUStudentQualification>(qualification);

            bool hasCertificate = (qualification.CertificateId != null && qualification.CertificateId > 0);

            uploadQualification.SourceCode = "P";
            uploadQualification.StatusCode = hasCertificate ? "C" : "U";

            CUAllQualification newQualification = await _apiService.PostJsonAsync<CUStudentQualification, CUAllQualification>(apiCUUrl, uploadQualification);

            if (newQualification == null)
            {
                return false;
            }

            if (hasCertificate)
            {
                Logger.Info("UploadQualification> Certificate :");

                //2. download Certificate from database
                var apiUrl = UrlExtension.Combine(WebApiBasePath, "/qualification/certificate/", qualification.CertificateId.ToString());

                HttpResponseMessage response = await _apiService.GetSteamAsync(apiUrl);

                if (response == null)
                {
                    return false;
                }
                StreamContent content = new StreamContent(await response.Content.ReadAsStreamAsync());
                //2. upload certificate to CU
                var apiDocumentUrl = UrlExtension.Combine(DocumentAPIPath, "upload");
                /*
                '*ApCd' - our application code - not required
                '*PerCd' - our person code
                '*StudId' - the student’s ID
                QualificationId - Hopefully we can include the AllQualificationId returned from /StudentService.svc/Qualifications
                DocumentType - “PEQS” This will be a new document type for us, “Document attached by student post enrolment”
                TheirReference - filename given by user
                TopicCode – “IMPORT”
                SubTopicCode – “QC” We will use the existing subtopic, and add it to the Import Topic “group”
                Subject - reference description given by user * this is an optional field, and I don’t think we have anything to add here ?
                CreatedBy – think this should be the student’s ID (again)
                Content-Length – of file not 100% sure, but isn’t this inferred from the filestream ?
                Content-Type – of file 
                 */

                //content.Headers.Add("*ApCd", "PQES");
                //content.Headers.Add("*PerCd", qualification.StudentId.ToString());
                content.Headers.Add("*StudId", qualification.StudentId.ToString());
                content.Headers.Add("QualificationId", newQualification.AllQualificationId.ToString());
                content.Headers.Add("DocumentType", "PQES");
                content.Headers.Add("TheirReference", response.Content.Headers.ContentDisposition.FileName);
                content.Headers.Add("TopicCode", "IMPORT");
                content.Headers.Add("SubTopicCode", "QC");
                content.Headers.Add("Subject", "Certificate");
                content.Headers.Add("CreatedBy", qualification.StudentId.ToString());
                content.Headers.Add("Content-Length", response.Content.Headers.ContentLength.ToString());
                content.Headers.Add("Content-Type", response.Content.Headers.ContentType.MediaType);

                //TODO:: Return type from document

                var result = await _apiService.PostHttpAsync(apiDocumentUrl, content);

                if (!result)
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// add Certificate  and get certificate Id
        /// </summary>
        /// <param name="qualification"></param>
        /// <returns></returns>
        public async Task<long> AddCertificate(CertificateDto certificate)
        {
            try
            {
                var apiUrl = UrlExtension.Combine(WebApiBasePath, "/qualification/certificate/");

                using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    var filecontent = new StreamContent(new MemoryStream(certificate.FileContent));
                    filecontent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(certificate.ContentType);
                    filecontent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    filecontent.Headers.ContentDisposition.FileName = certificate.FileName;
                    content.Add(filecontent, certificate.FileName, certificate.FileName);
                    // content.Add(fileContent);
                    return await _apiService.PostHttpAsync<long>(apiUrl, content);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Certificate Add Error", ex);
            }

            return 0;
        }

        /// <summary>
        /// Validate that valid user is dowlin
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public async Task<bool> ValidateCertificate(long certificate, long studentId)
        {

            var apiUrl = UrlExtension.Combine(WebApiBasePath, "/qualification/validcertificate/" + certificate.ToString() + "/" + studentId.ToString());

            return await _apiService.GetJsonAsync<bool>(apiUrl);

        }


        /// <summary>
        /// Down load the certificate from Database
        /// </summary>
        /// <param name="certificateId"></param>
        /// <returns></returns>
        public async Task<FileStreamResult> DownloadCertificate(long certificateId)
        {
            var apiUrl = UrlExtension.Combine(WebApiBasePath, "/qualification/certificate/", certificateId.ToString());

            HttpResponseMessage response = await _apiService.GetSteamAsync(apiUrl);

            Stream contents = await response.Content.ReadAsStreamAsync();
            var file = new FileStreamResult(contents, response.Content.Headers.ContentType.MediaType);
            file.FileDownloadName = response.Content.Headers.ContentDisposition.FileName.Replace("\"", "");
            return file;
        }

        /// <summary>
        /// Validate Qualification for duplicate
        /// validate existing and new qualifications
        /// </summary>
        /// <param name="qualification"></param>
        /// <returns></returns>
        public async Task<bool> ValidateQualification(QualificationDto qualification)
        {
            //return false if any duplicate record exists - match all columns
            var studentNewQualifications = await GetQualificationsByStudent(new QualificationSearchDto { StudentId = qualification.StudentId });
            var studentExistingQualifications = await GetExistingQualificationsByStudent(qualification.StudentId);

            var isRecordFound = studentNewQualifications.Concat(studentNewQualifications) //add the two data sets
                .Any(q => q.StudentQualificationId != qualification.StudentQualificationId &&
             q.StudentId == qualification.StudentId && q.Year == qualification.Year
          && q.QualificationTypeCode == qualification.QualificationTypeCode && q.SubjectCode == qualification.SubjectCode
          && q.Result == qualification.Result && q.SittingCode == qualification.SittingCode);

            return !isRecordFound;
        }
        /// <summary>
        /// Delete Qualification
        /// </summary>
        /// <param name="qualification"></param>
        /// <returns></returns>
        public async Task<bool> DeleteQualification(QualificationDto qualification)
        {
            //soft delete 
            var apiUrl = UrlExtension.Combine(WebApiBasePath, "/qualification/delete");

            var deleteQualification = Mapper.Map<Qualification>(qualification);
            var result = await _apiService.PostJsonAsync<Qualification>(apiUrl, deleteQualification);
            return result;
        }

        /// <summary>
        /// Update Qualification
        /// </summary>
        /// <param name="qualification"></param>
        /// <returns></returns>
        public async Task<long> UpdateQualification(QualificationDto qualification)
        {
            var apiUrl = UrlExtension.Combine(WebApiBasePath, "/qualification/update");

            var editQualification = Mapper.Map<Qualification>(qualification);
            editQualification.Status = (int)QualificationStatus.Pending;
            editQualification.ModifiedDate = DateTime.Today;
            editQualification.ModifiedBy = qualification.ActionUserName;
            //update to DB
            var updatedQualification = await _apiService.PostJsonAsync<Qualification, Qualification>(apiUrl, editQualification);

            return updatedQualification.StudentQualificationId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qualification"></param>
        /// <returns></returns>
        public async Task<long> ApproveQualification(QualificationDto qualification)
        {
            //update the status to Approve
            var apiUrl = UrlExtension.Combine(WebApiBasePath, "/qualification/approve");

            var apporveQualification = Mapper.Map<Qualification>(qualification);
            apporveQualification.Status = (int)QualificationStatus.Approved;
            apporveQualification.ActionDate = DateTime.Today;
            apporveQualification.ActionBy = qualification.ActionUserName;

            var updatedQualification = await _apiService.PostJsonAsync<Qualification, Qualification>(apiUrl, apporveQualification);
            return updatedQualification.StudentQualificationId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qualification"></param>
        /// <returns></returns>
        public async Task<long> RejectQualification(QualificationDto qualification)
        {
            var apiUrl = UrlExtension.Combine(WebApiBasePath, "/qualification/reject");

            var rejectQualification = Mapper.Map<Qualification>(qualification);
            rejectQualification.Status = (int)QualificationStatus.Rejected;
            rejectQualification.ActionDate = DateTime.Today;
            rejectQualification.ActionBy = qualification.ActionUserName;

            var updatedQualification = await _apiService.PostJsonAsync<Qualification, Qualification>(apiUrl, rejectQualification);
            return updatedQualification == null ? 0 : updatedQualification.StudentQualificationId;
        }

        #endregion

        #region private Methods
        private async Task<List<QualificationDto>> GetQualifications(QualificationSearchDto searchCriteria)
        {
            var apiUrl = UrlExtension.Combine(WebApiBasePath, "/qualification/get");

            var studentQualifications = Mapper.Map<List<QualificationDto>>(await _apiService.PostJsonAsync<QualificationSearchDto, List<Qualification>>(apiUrl, searchCriteria));

            return studentQualifications ?? new List<QualificationDto>();
        }
        #endregion


    }
}
