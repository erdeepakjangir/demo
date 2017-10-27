using Survey.Business.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Survey.Business.Services.Contracts
{
    public interface IStudentQualificationService
    {
        #region Public Methods

        Task<long> AddQualification(QualificationDto qualification);

        Task<long> UpdateQualification(QualificationDto qualification);

        Task<List<QualificationDto>> GetExistingQualificationsByStudent(long studentId);

        Task<List<QualificationDto>> GetQualificationsByStudent(QualificationSearchDto searchCriteria);

        Task<List<QualificationDto>> GetQualificationsByFaculty(QualificationSearchDto searchCriteria);

        Task<List<QualificationDto>> GetQualificationsBySearch(QualificationSearchDto searchCriteria);

        Task<ReportSummaryDto> GetReportBySearch(QualificationSearchDto searchCriteria);

        Task<bool> ValidateQualification(QualificationDto qualification);

        Task<bool> DeleteQualification(QualificationDto qualification);

        Task<long> ApproveQualification(QualificationDto qualification);

        Task<long> RejectQualification(QualificationDto qualification);

        Task<bool> UploadQualification(QualificationDto qualification);

        Task<FileStreamResult> DownloadCertificate(long certificateId);       

        Task<long> AddCertificate(CertificateDto certificate);

        Task<bool> ValidateCertificate(long certificate, long studentId);


        #endregion
    }
}
