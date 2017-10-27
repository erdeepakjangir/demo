using Survey.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.Business.Entities;
 
using Survey.Business.Services.Contracts;
using Survey.Business.Entities.Enums;
using System.Threading.Tasks;
using AutoMapper;
using Survey.Core.Enums;


namespace Survey.Web.Controllers
{

    [Authorize(Roles = "Planner, FacultyStaff")]
    public class ReportController : BaseController
    {


        #region Private Members
        readonly IQualificationService _qualificationService;
        readonly IStudentQualificationService _studentQualificationService;
        readonly IQualificationService _courseService;
        #endregion


        #region Service injection in constructor      
        public ReportController(IStudentQualificationService studentQualificationService, IQualificationService qualificationService, IQualificationService courseService)
        {

            _studentQualificationService = studentQualificationService;
            _qualificationService = qualificationService;
            _courseService = courseService;
        }
        #endregion

        ReportViewModel modelData = new ReportViewModel();

        public async Task<ActionResult> Index()
        {
            ReportViewModel model = new ReportViewModel();



            ViewBag.Years = await this._courseService.GetYears();

            model.SearchQualification = new SearchViewModel { Year = DateTime.Today.Year };

            return View(model);
        }
        [HttpPost]

        public async Task<ActionResult> Search(ReportSearchViewModel searchQualification)
        {
            ReportViewModel model = new ReportViewModel();
            if (searchQualification.Year > 0)
            {
                searchQualification.SubmittedFromDate = new DateTime(searchQualification.Year, 1, 1);
                searchQualification.SubmittedToDate = new DateTime(searchQualification.Year, 12, 31);

            }
            else
            {
                searchQualification.SubmittedFromDate = searchQualification.SubmittedFromDate;
                searchQualification.SubmittedToDate = searchQualification.SubmittedToDate;
            }

            if (User.IsInRole(UserRole.FacultyStaff.ToString()))
            {
                searchQualification.FacultyCode = new string[] { base.LoggedInUserFacultyCode };
                model.IsFacultyView = true;

            }
            else // if Admin view  faculty wise
                if (searchQualification.FacultyCode != null && searchQualification.FacultyCode.Any(f => !string.IsNullOrWhiteSpace(f)))
            {
                model.IsFacultyView = true;
            }


            //Get Report Data
            model.ReportSummary = await _studentQualificationService.GetReportBySearch(Mapper.Map<QualificationSearchDto>(searchQualification));


            return PartialView("_report", model);


        }

    }

}

