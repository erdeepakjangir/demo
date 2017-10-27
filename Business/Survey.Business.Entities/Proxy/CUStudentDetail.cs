using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Entities.Proxy
{
    public class CUStudentDetail
    {
        #region Properties  
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public int CourseStage { get; set; }
        public string ModeOfStudy { get; set; }
        public long StudentId { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; } 
        public string FacultyCode { get; set; }
        public string QualificationAim { get; set; }
        #endregion

    }
}
