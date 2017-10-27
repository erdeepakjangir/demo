using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Survey.Business.Services.Contracts
{
    public interface IAPIService
    {

        #region Public Methods
        Task<T> GetJsonAsync<T>(string resourceUrl);

        Task<bool> PostJsonAsync<T>(string postUrl, T payload);

        Task<R> PostJsonAsync<T, R>(string postUrl, T payload);

        Task<R> PostHttpAsync<R>(string postUrl, HttpContent payload);

        Task<bool> PostHttpAsync(string postUrl, HttpContent payload);

        Task<HttpResponseMessage> GetSteamAsync(string postUrl);

        #endregion




    }
}
