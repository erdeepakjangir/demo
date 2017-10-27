using Survey.Business.Entities.Enums;
using Survey.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Entities
{
    public class QualificationDto : BaseDto
    {
        #region Properties   

        public long StudentQualificationId { get; set; }
        public long StudentId { get; set; }
        public string StudentName { get; set; }

        public string FacultyCode { get; set; }

        public int Year { get; set; }
        public string QualificationTypeCode { get; set; }
        public string QualificationTitle { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectTitle { get; set; }

        public string Result { get; set; }
        public char SittingCode { get; set; }
        public string SittingTitle { get; set; }
        public DateTime SubmittedDate { get; set; }

        public QualificationStatus Status { get; set; }
        public long? CertificateId { get; set; }
        //public CertificateDto Certificate { get; set; }

        public string Remark { get; set; }

        public string ActionUserName { get; set; }

        #endregion
    }
}
