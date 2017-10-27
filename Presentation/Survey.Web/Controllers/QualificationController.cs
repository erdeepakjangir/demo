using AutoMapper;
using Survey.Business.Entities;
using Survey.Business.Entities.Enums;
using Survey.Business.Services.Contracts;
using Survey.Core.Enums;
using Survey.Web.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Survey.Web.Controllers
{
    public class QualificationController : BaseController
    {
        #region Private Members
        readonly IStudentQualificationService _studentQualificationService;
        readonly IStudentService _userService;
        readonly IQualificationService _qualificationService;
        const long MaxRemarkSize = 3800; //Keeping it a less value because special char take more bytes in database.
        #endregion

        #region Service injection in constructor      
        public QualificationController(IStudentQualificationService studentQualificationService, IStudentService userservice, IQualificationService qualificationService)
        {
            _studentQualificationService = studentQualificationService;
            _userService = userservice;
            _qualificationService = qualificationService;
        }
        #endregion

        #region UGStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "UGStudent")]
        public async Task<JsonResult> Add(QualificationViewModel addQualification, HttpPostedFileBase certificate)
        {
            try
            {
                //Add Certificate
                if (this.ModelState.IsValid)
                {
                    var newQuaifications = Mapper.Map<QualificationDto>(addQualification);

                    newQuaifications.StudentId = this.LoggedInUserId;
                    newQuaifications.StudentName = User.Identity.Name;
                    newQuaifications.FacultyCode = this.LoggedInUserFacultyCode;
                    newQuaifications.Status = QualificationStatus.Pending;

                    string[] names = await _qualificationService.GetNamesByCode(addQualification.QualificationTypeCode, addQualification.SubjectCode, addQualification.Result, addQualification.SittingCode);
                    newQuaifications.QualificationTitle = names[0];
                    newQuaifications.SubjectTitle = names[1];
                    newQuaifications.Result = names[2];
                    newQuaifications.SittingTitle = names[3];

                    newQuaifications.ActionUserName = base.LoggedInUserName;

                    //duplicate Validation
                    var isValid = await _studentQualificationService.ValidateQualification(newQuaifications);

                    if (!isValid)
                    {
                        return Json(new { Status = false, Message = Resources.ModelValidations.Message_Qualification_Already_Exists }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //clear certificate Id if posted
                        newQuaifications.CertificateId = null;
                        //now check file
                        if (certificate != null)
                        {
                            //set certificate Id
                            var newCertificateId = await UploadCertificate(certificate);

                            if (newCertificateId == null)
                            {
                                return Json(new { Status = false, Message = ViewData["errorMessage"] });
                            }
                            else
                            { //update certificate Id
                                newQuaifications.CertificateId = newCertificateId;
                            }
                        }
                        //now Add Qualification
                        var result = await _studentQualificationService.AddQualification(newQuaifications);
                        if (result > 0)
                        {
                            return Json(new { Status = true, Message = Resources.ModelValidations.Message_Qualification_Add_Success }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error("Add Qualification", ex);

            }
            return Json(new { Status = false, Message = Resources.ModelValidations.Message_Qualification_Add_Error }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "UGStudent")]
        public async Task<JsonResult> AddCertificate(HttpPostedFileBase certificate)
        {
            try
            {
                if (certificate != null)
                {
                    //set certificate Id
                    var newCertificateId = await UploadCertificate(certificate);

                    if (newCertificateId == null)
                    {
                        return Json(new { Status = false, Message = ViewData["errorMessage"] });
                    }
                    else
                    { //update certificate Id
                        return Json(new { Status = true, CertificateId = newCertificateId });
                    }
                }
                else
                {
                    return Json(new { Status = false, Message = Resources.ModelValidations.Validation_Certificate_Required });
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error("Add Certificate", ex);
            }

            return Json(new { Status = false, Message = Resources.ModelValidations.Message_Certificate_Error }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Download Certificate
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "UGStudent,Planner, FacultyStaff")]
        public async Task<ActionResult> DownloadCertificate(long id)
        {
            //Validate a student is downloading its own certificate
            if (User.IsInRole(UserRole.UGStudent.ToString()) && !await _studentQualificationService.ValidateCertificate(id, base.LoggedInUserId))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            return await _studentQualificationService.DownloadCertificate(id);

        }
        /// <summary>
        /// Edit View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [Authorize(Roles = "UGStudent,Planner, FacultyStaff")]
        public async Task<ActionResult> Edit(long id, long? studentId)
        {
            QualificationSearchDto searchCriteria = new QualificationSearchDto
            {
                StudentQualificationId = id,
                Status = new QualificationStatus?[] { QualificationStatus.Pending }
            };

            if (User.IsInRole(UserRole.UGStudent.ToString()))
            {
                searchCriteria.StudentId = base.LoggedInUserId;

            }
            else
            {
                searchCriteria.StudentId = studentId;
            }

            //Get Record
            var model = Mapper.Map<QualificationViewModel>((await _studentQualificationService.GetQualificationsByStudent(searchCriteria))[0]);

            await PopulateDropdowns(model.QualificationTypeCode);

            return PartialView("_Edit", model);
        }


        /// <summary>
        /// Submit Edit View/ Save Qualification
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "UGStudent, Planner, FacultyStaff")]
        public async Task<ActionResult> Edit(QualificationViewModel model)
        {
            try
            {
                await PopulateDropdowns(model.QualificationTypeCode);

                if (ModelState.IsValid)
                {
                    var updateQuaifications = Mapper.Map<QualificationDto>(model);

                    updateQuaifications.SubmittedDate = DateTime.Today;

                    if (User.IsInRole(UserRole.UGStudent.ToString()))
                    {
                        updateQuaifications.StudentId = base.LoggedInUserId;
                        updateQuaifications.FacultyCode = this.LoggedInUserFacultyCode;
                    }
                    else if (User.IsInRole(UserRole.FacultyStaff.ToString()))
                    {
                        updateQuaifications.FacultyCode = base.LoggedInUserFacultyCode;
                    }


                    string[] names = await _qualificationService.GetNamesByCode(model.QualificationTypeCode, model.SubjectCode, model.Result, model.SittingCode);
                    updateQuaifications.QualificationTitle = names[0];
                    updateQuaifications.SubjectTitle = names[1];
                    updateQuaifications.Result = names[2];
                    updateQuaifications.SittingTitle = names[3];

                    updateQuaifications.ActionUserName = base.LoggedInUserName;
                    //duplicate Validation
                    var isValid = await _studentQualificationService.ValidateQualification(updateQuaifications);

                    if (isValid)
                    {
                        await _studentQualificationService.UpdateQualification(updateQuaifications);
                        //Add success messge
                        ViewBag.SuccessMessage = Resources.ModelValidations.Message_Qualification_Edit_Success;
                    }
                    else
                    {
                        ModelState.AddModelError("", Resources.ModelValidations.Message_Qualification_Already_Exists);

                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.ModelValidations.Message_Edit_All_Fields_Required);
                }

                return PartialView("_Edit", model);

            }
            catch (Exception ex)
            {
                this.Logger.Error("Edit Qualification", ex);
                ModelState.AddModelError("", Resources.ModelValidations.Message_Qualification_Edit_Error);
            }
            return PartialView("_Edit", model);
        }

        // [ValidateAntiForgeryToken]
        [Authorize(Roles = "UGStudent")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                QualificationDto qualification = new QualificationDto { StudentQualificationId = id, StudentId = this.LoggedInUserId, ActionUserName = this.LoggedInUserName };

                var result = await _studentQualificationService.DeleteQualification(qualification);
                if (result)
                {

                    return Json(new { Status = true, Message = Resources.ModelValidations.Message_Qualification_Delete_Success }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { Status = false, Message = Resources.ModelValidations.Message_Qualification_Delete_Fail }, JsonRequestBehavior.AllowGet);
                }

            }


            catch (Exception ex)
            {
                this.Logger.Error("Delete Qualification", ex);
            }
            return Json(new { Status = false, Message = Resources.ModelValidations.Message_Qualification_Delete_Fail }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Get GetPendingQualifications to refresh grid
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "UGStudent, Planner, FacultyStaff")]
        public async Task<ActionResult> GetPendingQualifications()
        {
            QualificationSearchDto search = new QualificationSearchDto { StudentId = base.LoggedInUserId, Status = new QualificationStatus?[] { QualificationStatus.Pending } };
            var pendingQuaifications = Mapper.Map<IEnumerable<QualificationViewModel>>(await this._studentQualificationService.GetQualificationsByStudent(search));

            return PartialView("_PendingQualificationView", pendingQuaifications);
        }

        #endregion

        #region Planner Staff

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Planner, FacultyStaff")]
        public async Task<JsonResult> Approve(long id, long studentId)
        {
            try
            {
                //Get the record
                QualificationSearchDto searchCriteria = new QualificationSearchDto { StudentQualificationId = id, StudentId = base.LoggedInUserId, Status = new QualificationStatus?[] { QualificationStatus.Pending } };
                if (User.IsInRole(UserRole.FacultyStaff.ToString()))
                {
                    searchCriteria.FacultyCode = new string[] { base.LoggedInUserFacultyCode };
                }

                var pendingQuaification = (await this._studentQualificationService.GetQualificationsByStudent(searchCriteria));

                //upload qualification to CU  & certificate
                if (pendingQuaification != null && pendingQuaification.Count == 1 && await this._studentQualificationService.UploadQualification(pendingQuaification[0]))
                {
                    //approve
                    var qualification = pendingQuaification[0];
                    qualification.ActionUserName = LoggedInUserName;

                    if (await this._studentQualificationService.ApproveQualification(qualification) > 0)
                    {
                        return Json(new { Status = true, Message = Resources.ModelValidations.Message_Qualification_Apporve_Success }, JsonRequestBehavior.AllowGet);
                    }

                }
                //if fail
                return Json(new { Status = false, Message = Resources.ModelValidations.Message_Qualification_Apporve_Fail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                this.Logger.Error("Approve Qualification", ex);
                return Json(new { Status = false, Message = string.Format("{0}</br>{1}", Resources.ModelValidations.Message_Qualification_Apporve_Fail, ex.Message) }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="studentId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Planner, FacultyStaff")]
        public async Task<JsonResult> Reject(long id, long studentId, string remark)
        {
            QualificationViewModel model = new QualificationViewModel();
            try
            {

                byte[] remarkBytes = Encoding.ASCII.GetBytes(remark);


                if (string.IsNullOrWhiteSpace(remark) || remark.Length > MaxRemarkSize || remarkBytes.Length > MaxRemarkSize)
                {
                    return Json(new { Status = false, Message = Resources.ModelValidations.Message_Qualification_Reject_Remark_Len }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    //update remark
                    var qualification = new QualificationDto { StudentQualificationId = id, StudentId = studentId, Remark = remark, ActionUserName = this.LoggedInUserName };

                    if (User.IsInRole(UserRole.FacultyStaff.ToString()))
                    {
                        qualification.FacultyCode = base.LoggedInUserFacultyCode;
                    }

                    var result = await this._studentQualificationService.RejectQualification(qualification);

                    if (result > 0)
                    {
                        return Json(new { Status = true, Message = Resources.ModelValidations.Message_Qualification_Reject_Success }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Status = false, Message = Resources.ModelValidations.Message_Qualification_Reject_Fail }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error("Reject Qualification", ex);
                return Json(new { Status = false, Message = string.Format("{0}</br>{1}", Resources.ModelValidations.Message_Qualification_Reject_Fail, ex.Message) }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchQualification"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Planner, FacultyStaff")]
        public async Task<ActionResult> Search(SearchViewModel searchQualification)
        {
            try
            {
                if (User.IsInRole(UserRole.FacultyStaff.ToString()))
                {
                    searchQualification.FacultyCode = new string[] { base.LoggedInUserFacultyCode };
                }

                var vmSearchResult = Mapper.Map<List<QualificationViewModel>>(await _studentQualificationService.GetQualificationsBySearch(Mapper.Map<QualificationSearchDto>(searchQualification)));

                return PartialView("_QualificationView", vmSearchResult);

            }
            catch (Exception ex)
            {
                this.Logger.Error("Search Qualification", ex);
                return Json(new { Status = false, Message = string.Format("{0}</br>{1}", Resources.ModelValidations.Message_Qualification_Search_Fail, ex.Message) }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Planner, FacultyStaff")]
        public async Task<ActionResult> StudentQualification(long id)
        {
            var vmStudent = new StudentViewModel();

            if (id > 0)
            {
                ///Get user profile by studentId
                var profile = await _userService.GetUserProfile(id);

                if (profile != null)
                {
                    vmStudent.StudentId = id;
                    vmStudent.StudentName = profile.FullName;
                    vmStudent.CourseCode = profile.CourseCode;
                    vmStudent.CourseTitle = profile.CourseTitle;
                    vmStudent.Mode = profile.Mode;
                    vmStudent.QualificationAim = profile.QualAim;

                }
                //Get from CU
                vmStudent.ExistingQualifications = Mapper.Map<List<QualificationViewModel>>(await this._studentQualificationService.GetExistingQualificationsByStudent(vmStudent.StudentId));

                QualificationSearchDto searchCriteria = new QualificationSearchDto { StudentId = id };
                if (User.IsInRole(UserRole.FacultyStaff.ToString()))
                {
                    searchCriteria.FacultyCode = new string[] { base.LoggedInUserFacultyCode };
                }

                //Get 
                searchCriteria.Status = new QualificationStatus?[] { QualificationStatus.Pending, QualificationStatus.Rejected };
                var studentQualifications = Mapper.Map<List<QualificationViewModel>>(await this._studentQualificationService.GetQualificationsByStudent(searchCriteria));

                vmStudent.PendingQualifications = studentQualifications.FindAll(q => q.Status == QualificationStatus.Pending);
                vmStudent.RejectedQualifications = studentQualifications.FindAll(q => q.Status == QualificationStatus.Rejected);
            }
            return View(vmStudent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Planner, FacultyStaff")]
        public async Task<ActionResult> StudentExistingQualification(long id)
        {
            var vmStudent = new StudentViewModel();
            if (id > 0)
            {
                QualificationSearchDto searchCriteria = new QualificationSearchDto { StudentId = id };
                if (User.IsInRole(UserRole.FacultyStaff.ToString()))
                {
                    searchCriteria.FacultyCode = new string[] { base.LoggedInUserFacultyCode };
                }
                searchCriteria.Status = new QualificationStatus?[] { QualificationStatus.Approved };

                vmStudent.ExistingQualifications = Mapper.Map<List<QualificationViewModel>>(await this._studentQualificationService.GetExistingQualificationsByStudent(id));

                vmStudent.StudentId = id;
            }
            return PartialView("_Existing", vmStudent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Planner, FacultyStaff")]
        public async Task<ActionResult> StudentPendingQualification(long id)
        {
            var vmStudent = new StudentViewModel();

            if (id > 0)
            {
                QualificationSearchDto searchCriteria = new QualificationSearchDto { StudentId = id };
                if (User.IsInRole(UserRole.FacultyStaff.ToString()))
                {
                    searchCriteria.FacultyCode = new string[] { base.LoggedInUserFacultyCode };
                }
                searchCriteria.Status = new QualificationStatus?[] { QualificationStatus.Pending };

                vmStudent.StudentId = id;
                vmStudent.PendingQualifications = Mapper.Map<List<QualificationViewModel>>(await this._studentQualificationService.GetQualificationsByStudent(searchCriteria));

            }
            return PartialView("_Pending", vmStudent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Planner, FacultyStaff")]
        public async Task<ActionResult> StudentRejectedQualification(long id)
        {
            var vmStudent = new StudentViewModel();

            QualificationSearchDto searchCriteria = new QualificationSearchDto { StudentId = id };
            if (User.IsInRole(UserRole.FacultyStaff.ToString()))
            {
                searchCriteria.FacultyCode = new string[] { base.LoggedInUserFacultyCode };
            }
            searchCriteria.Status = new QualificationStatus?[] { QualificationStatus.Rejected };

            vmStudent.StudentId = id;
            vmStudent.RejectedQualifications = Mapper.Map<List<QualificationViewModel>>(await this._studentQualificationService.GetQualificationsByStudent(searchCriteria));


            return PartialView("_Rejected", vmStudent);
        }

        #endregion

        /// <summary>
        ///On course  Drop down change , populate Subject , Result and Sitting
        /// </summary>
        /// <param name="qualificationCode"></param>
        /// <returns></returns>
        [OutputCache(CacheProfile = "Cache1Hour")]
        [Authorize(Roles = "UGStudent, Planner, FacultyStaff")]
        public async Task<JsonResult> GetSubjectAndResult(string qualificationCode)
        {
            var subjects = await this._qualificationService.GetSubjects(qualificationCode);
            var results = await this._qualificationService.GetResults(qualificationCode);
            var sittings = await this._qualificationService.GetSittings();

            return Json(new { Subjects = subjects, Results = results }, JsonRequestBehavior.AllowGet);
        }


        #region Private Method
        private async Task<bool> PopulateDropdowns(string qualificationCode)
        {

            var qualifications = await this._qualificationService.GetQualifications();

            if (!string.IsNullOrWhiteSpace(qualificationCode))
            {
                var subjects = await this._qualificationService.GetSubjects(qualificationCode);
                var results = await this._qualificationService.GetResults(qualificationCode);
                ViewBag.Subjects = new SelectList(subjects, "SubjectCode", "SubjectTitle");
                ViewBag.Results = new SelectList(results, "Result", "Result");
            }

            var sittings = await this._qualificationService.GetSittings();
            ViewBag.Years = await this._qualificationService.GetYears();

            ViewBag.Qualifications = new SelectList(qualifications, "Code", "Title", 0);
            ViewBag.Sittings = new SelectList(sittings, "SittingCode", "SittingTitle");
            return true;
        }


        /// <summary>
        /// Upload Certificate to API
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        
        private async Task<long?> UploadCertificate(HttpPostedFileBase certificate)
        {
            try
            {                // check the file size (max 5 Mb)
                if (certificate.ContentLength > Convert.ToInt64(ConfigurationManager.AppSettings["MaxCertSizeMB"].ToString()) * 1024 * 1024)
                {
                    ViewData["errorMessage"] = Resources.ModelValidations.Validation_Certificate_MaxSize + " " + ConfigurationManager.AppSettings["MaxCertSizeMB"] + " MB";
                    return null;
                }
                // check the file size (min 100 bytes)
                if (certificate.ContentLength < 100)
                {
                    ViewData["errorMessage"] = Resources.ModelValidations.Validation_Certificate_MinSize;
                    return null;
                }
                // check file extension

                string extension = Path.GetExtension(certificate.FileName).ToLower();
                string allowedExtenstions = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["CertExtenstion"]);
                List<string> extenstionList = new List<string>(allowedExtenstions.Split(','));

                if (!extenstionList.Contains(extension))
                {
                    ViewData["errorMessage"] = Resources.ModelValidations.Validation_Certificate_Extenstions + allowedExtenstions;
                    return null;
                }

                string certificateName;
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] filesName = certificate.FileName.Split(new char[] { '\\' });
                    certificateName = filesName[filesName.Length - 1];
                }
                else
                {
                    certificateName = certificate.FileName;
                }

                Logger.Info("File Received");
                CertificateDto cert = new CertificateDto
                {
                    FileName = certificateName,
                    FileContent = new byte[certificate.ContentLength - 1],
                    ContentType = certificate.ContentType
                };

                certificate.InputStream.Read(cert.FileContent, 0, cert.FileContent.Length);

                Logger.Info("uploading file");
                var certificateId = await _studentQualificationService.AddCertificate(cert);

                if (certificateId > 0)
                {
                    return certificateId;
                }
                else
                {
                    ViewData["errorMessage"] = Resources.ModelValidations.Message_Certificate_Fail;
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error("Add Certificate", ex);
                ViewData["errorMessage"] = Resources.ModelValidations.Message_Certificate_Error;
            }
            return null;
        }

        #endregion
    }
}