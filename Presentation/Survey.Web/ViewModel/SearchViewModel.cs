using Survey.Business.Entities.Enums;
using Survey.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Survey.Web.ViewModel
{
    public class SearchViewModel
    {
        #region Properties

        public string[] QualificationCode { get; set; }
        public QualificationStatus?[] Status { get; set; }
        public string[] FacultyCode { get; set; }        
        public long? StudentId { get; set; }
        public DateTime? SubmittedFromDate { get; set; }
        public DateTime? SubmittedToDate { get; set; }
        public int Year { get; set; }

        #endregion

    }
}