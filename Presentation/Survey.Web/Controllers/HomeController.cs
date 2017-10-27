using AutoMapper;
using Survey.Business.Entities;
using Survey.Business.Entities.Enums;
using Survey.Business.Entities.Survey;
using Survey.Business.Services.Contracts;
using Survey.Business.Services.Implementation;
using Survey.Core.Enums;
using Survey.Presentaion.Helpers;
using Survey.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Survey.Web.Controllers
{

    public class HomeController : BaseController
    {
        #region private Members  

        readonly ISurveyService _surveyService;
        readonly IQuestionBankService _QuestionbankService;


        #endregion

        #region Service injection in constructor      
        public HomeController(ISurveyService surveyService, IQuestionBankService questionbankService)
        {

            _surveyService = surveyService;
            _QuestionbankService = questionbankService;


        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Home page for all user. Based on role navigate user  to appropriate view.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole(UserRole.User.ToString()))
            {

                //Add Qualification code
                var surveyList = Mapper.Map<IEnumerable<SurveyViewModel>>(await this._surveyService.GetSurvey());

                var QuestionList = Mapper.Map<IEnumerable<QuestionBankViewModel>>(await this._QuestionbankService.GetQuestion());
                return View("Index",  QuestionList);
            }


            return RedirectToAction("AccessDenied", "Error");
        }

        public async Task<RedirectToRouteResult> AddQue(QuestionbankDto que)
        {           
            //_QuestionbankService.AddQuetion(que);
            await this._QuestionbankService.AddQuetion(que);
            return RedirectToAction("Index", "Home");
        }

        #endregion

    }
}