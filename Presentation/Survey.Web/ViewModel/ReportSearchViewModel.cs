using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Survey.Web.ViewModel
{
    public class ReportSearchViewModel
    {

        #region Properties


        public string[] FacultyCode { get; set; }
        [Required(ErrorMessage ="From Date Required")]
        public DateTime SubmittedFromDate { get; set; }
        [Required(ErrorMessage ="To Date Required")]
        public DateTime SubmittedToDate { get; set; }

        public int Year { get; set; }

        #endregion
    }
}