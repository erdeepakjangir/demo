using System;
using System.Collections.Generic; 
using System.Security.Claims;
using System.Threading.Tasks;

namespace Survey.Business.Services.Contracts
{
    public interface IActiveDirectoryService
    {
        #region Public Methods
        Task<List<Claim>> ValidateUser(string domain,string username, string password);
        #endregion
    }
}
