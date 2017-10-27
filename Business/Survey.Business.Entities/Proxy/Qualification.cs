using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Entities.Proxy
{
    public class Qualification
    {
        #region Properties 
        public long StudentId { get; set; }
        public int Year { get; set; }
        public string QualificationTypeCode { get; set; }
        public string QualificationTitle { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string Result { get; set; }
        public string SittingCode { get; set; }
        public string SittingName { get; set; }


        public long StudentQualificationId { get; set; }
        public string StudentName { get; set; }
        public string FacultyCode { get; set; } 
 
     
        public int Status { get; set; }
        public int? CertificateId { get; set; }
        public string Remark { get; set; }

        public string ActionBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ActionDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion
    }
}
