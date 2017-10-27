using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey.Web.ViewModel
{
    public class StaffPlannerViewModel
    {
        #region Properties


        public IEnumerable<QualificationViewModel> Qualifications { get; set; }

        

        public SearchViewModel SearchQualification { get; set; }

        #endregion
    }
}