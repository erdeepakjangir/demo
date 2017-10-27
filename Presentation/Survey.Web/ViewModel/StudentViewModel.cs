using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey.Web.ViewModel
{
    public class StudentViewModel
    {
        #region Properties 
        public long StudentId { get; set; }
        public string StudentName { get; set; }
        public IEnumerable<QualificationViewModel> ExistingQualifications { get; set; }
        public IEnumerable<QualificationViewModel> PendingQualifications { get; set; }
        public IEnumerable<QualificationViewModel> RejectedQualifications { get; set; }

        public SelectList QualificationCodes { get; set; }
        public SelectList QualificationTitles { get; set; }    

       
        public SearchViewModel SearchQualification { get; set; }         
        
        public QualificationViewModel AddQualification{ get; set; }
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string QualificationAim { get; set; }
        public string Mode { get; set; }

        



        #endregion

    }
}