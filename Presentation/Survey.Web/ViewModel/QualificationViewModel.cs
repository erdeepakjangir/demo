using Survey.Business.Entities.Enums;
using Survey.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Survey.Web.ViewModel
{
    public class QualificationViewModel
    {
        #region Properties
        public long StudentQualificationId { get; set; }

        public long StudentId { get; set; }

        public string StudentName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelValidations), ErrorMessageResourceName = "Validation_Year_Required")]
        public int Year { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelValidations), ErrorMessageResourceName = "Validation_Qualification_Required")]
        [Display(Name = "Qualification")]
        public string QualificationTypeCode { get; set; }

        [Display(Name = "Qualification")]
        public string QualificationTitle { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelValidations), ErrorMessageResourceName = "Validation_Subject_Required")]
        [Display(Name = "Subject")]
        public string SubjectCode { get; set; }

        [Display(Name = "Subject")]
        public string SubjectTitle { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelValidations), ErrorMessageResourceName = "Validation_Sitting_Required")]
        [Display(Name = "Sitting")]
        public char SittingCode { get; set; }

        [Display(Name = "Sitting")]
        public string SittingTitle { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.ModelValidations), ErrorMessageResourceName = "Validation_Result_Required")]
        [Display(Name = "Result")]

        public string Result { get; set; }

        public QualificationStatus Status { get; set; }

        public long? CertificateId { get; set; }   

        public HttpPostedFileBase Certificate { get; set; }

        public string FacultyCode { get; set; }

        public string FacultyName { get; set; }

        public DateTime SubmittedDate { get; set; }
        [MaxLength(400, ErrorMessageResourceType = typeof(Resources.ModelValidations), ErrorMessageResourceName = "Validation_Remark_Length")]
        public string Remark { get; set; }
        public string Filename { get; set; }




        #endregion

    }
}


