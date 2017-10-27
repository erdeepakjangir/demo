using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Entities.Proxy
{
    public class CUStudentQualification
    {
        #region Properties 
        public long StudentId { get; set; }
        public int Year { get; set; }
        public string Qualification { get; set; }
        public string Subject { get; set; }
        public string Result { get; set; }
        public string Sitting { get; set; }
    
        
        public string QualificationTypeCode { get; set; }
        public string SittingCode { get; set; }
        public string SubjectCode { get; set; }
        public string SourceCode { get; set; }
        public string StatusCode { get; set; }

        #endregion
    }
}
