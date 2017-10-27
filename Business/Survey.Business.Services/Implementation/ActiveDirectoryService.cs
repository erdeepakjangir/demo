
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Survey.Business.Services.Contracts;
using Survey.Business.Entities.Enums;

using Survey.Core.Contracts;
using Survey.Data.DataAccess.Repositories;

namespace Survey.Business.Services.Implementation
{
    public class ActiveDirectoryService : BaseService, IActiveDirectoryService
    {
        #region Private Members   
        private IStudentService _studentService;
        private IQualificationService _qualificationService;
        #endregion

        #region Service injection in constructor      
        public ActiveDirectoryService(IUnitOfWork unitOfWork, IExceptionReporter exceptionReporter, IStudentService studentService, IQualificationService qualificationService)
              : base(unitOfWork, exceptionReporter)
        {
            _studentService = studentService;
            _qualificationService = qualificationService;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// This method will validate the user against active directory and retrieve information from AD to identify user.
        /// It will return set of claim for valid user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<List<Claim>> ValidateUser(string domain, string username, string password)
        {
            try
            {

                var ldapConnectionString = string.Format("GC://10.10.52.100/dc={0},dc=ads,dc=valuelabs,dc=net", domain);

                var domainSearch = new DirectoryEntry(ldapConnectionString);
                var search1 = new DirectorySearcher(domainSearch);
                var res = search1.FindAll();

                var domainAndUsername = domain + @"\" + username;


                DirectoryEntry dirEntry;


                dirEntry = new DirectoryEntry(ldapConnectionString, domainAndUsername, password, System.DirectoryServices.AuthenticationTypes.Secure);


                var search = new DirectorySearcher(dirEntry) { Filter = "(samaccountname=" + username + ")" };

                Logger.Debug("ValidateUser: Active directory searching...");

                var result = search.FindOne();

                if (null != result)
                {
                    Logger.Debug("ValidateUser: Valid User  found");

                    var Claims = new List<Claim>();

                    //TODO: Remove loop code once tested
                    string str = "";



                    Logger.Debug("ValidateUser: Active directory\n" + str);
                    //
                    string loggedInUserName = "";
                    if (result.Properties["samaccountname"] != null && result.Properties["samaccountname"].Count > 0 &&
                       result.Properties["samaccountname"][0] != null)
                    {
                        loggedInUserName = result.Properties["samaccountname"][0].ToString();
                        Claims.Add(new Claim(ClaimTypes.WindowsAccountName, loggedInUserName));
                        //required for identiy and AFT
                        Claims.Add(new Claim(ClaimTypes.NameIdentifier, loggedInUserName));
                    }

                    if (result.Properties["displayname"] != null && result.Properties["displayname"].Count > 0 &&
                        result.Properties["displayname"][0] != null)
                    {

                        Claims.Add(new Claim(ClaimTypes.Name, result.Properties["displayname"][0].ToString()));
                        Claims.Add(new Claim(ClaimTypes.GivenName, result.Properties["displayname"][0].ToString()));
                    }
                    if (result.Properties["mail"] != null && result.Properties["mail"].Count > 0 &&
                        result.Properties["mail"][0] != null)
                    {
                        Claims.Add(new Claim(ClaimTypes.Email, result.Properties["mail"][0].ToString()));
                    }

                    string department = String.Empty;
                    //get department
                    if (result.Properties["department"] != null && result.Properties["department"].Count > 0 &&
                        result.Properties["department"][0] != null)
                    {
                        department = result.Properties["department"][0].ToString();

                        Claims.Add(new Claim(ClaimConfiguration.Department.ToString(), department));
                    }

                    return Claims;
                }
                Logger.Debug("ValidateUser: No matching user found");

            }
            catch (Exception ex)
            {
                Logger.Error("ValidateUser: Exception" + ex.Message);
                //  base.ExceptionReporter.Report(ex, "Exception occurred in Authentication");
                // throw;
            }
            return null;
        }
        #endregion
    }
}
