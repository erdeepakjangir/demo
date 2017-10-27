using Survey.Business.Entities.Enums;
using Survey.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Entities
{
    public class QualificationSearchDto:BaseDto
    {
        #region Properties
        public long StudentQualificationId { get; set; }
        public string[] QualificationCode { get; set; }       
        public QualificationStatus?[] Status { get; set; }
        public string[] FacultyCode { get; set; }
        public long? StudentId { get; set; }
        public DateTime? SubmittedFromDate { get; set; }
        public DateTime? SubmittedToDate { get; set; }

        #endregion
    }
}
