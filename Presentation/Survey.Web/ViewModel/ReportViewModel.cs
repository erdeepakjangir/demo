using Survey.Business.Entities;
using Survey.Business.Entities.Enums;
using Survey.Core.Enums;
using Survey.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace Survey.Web.ViewModel
{
    public class ReportViewModel
    {
        public ReportSummaryDto ReportSummary { get; set; }

        public SearchViewModel SearchQualification { get; set; }

        public bool IsFacultyView { get; set; }
    }

}