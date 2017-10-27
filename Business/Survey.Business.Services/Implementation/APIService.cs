using Survey.Business.Services.Contracts;
using Survey.Core.Contracts;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Survey.Data.DataAccess.Repositories;

namespace Survey.Business.Services.Implementation
{
    public class APIService : BaseService, IAPIService
    {

        #region Private Members
        const string ApplicationJson = "application/json";
        const string ApplicationOctet = "application/octet-stream";
        #endregion

        #region Properties

        #endregion

        #region Constructors

        public APIService(IUnitOfWork unitOfWork, IExceptionReporter exceptionReporter)
            : base(unitOfWork, exceptionReporter)
        {

        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Get async response for HTTP get request
        /// </summary>
        /// <param name="resourceUrl"></param>
        /// <returns></returns>
        public async Task<T> GetJsonAsync<T>(string resourceUrl)
        {
            base.Logger.Info("GetJsonAsync<T: resourceUrl" + resourceUrl);

            T responseObject = default(T);
            try
            {
                using (HttpClient client = this.GetHttpClient(resourceUrl))
                {
                    HttpResponseMessage response = await client.GetAsync(resourceUrl).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        String result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        if (!string.IsNullOrWhiteSpace(result))
                        {
                            responseObject = JsonConvert.DeserializeObject<T>(result);
                        }
                    }
                    else
                    {
                        base.Logger.Error("Error in Service Call for url " + resourceUrl + "  ReasonPhrase : " + response.ReasonPhrase);

                    }
                }
            }
            catch (Exception ex)
            {
                base.Logger.Error("Exception in Service Call : " + resourceUrl, ex);
            }
            return responseObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="postUrl"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task<bool> PostJsonAsync<T>(string postUrl, T payload)
        {
            base.Logger.Info("PostJsonAsync<T>: postUrl" + postUrl);

            try
            {
                using (HttpClient client = this.GetHttpClient(postUrl))
                {

                    string stringPayload = JsonConvert.SerializeObject(payload);
                    // HTTP POST
                    StringContent content = new StringContent(stringPayload);

                    content.Headers.ContentType = new MediaTypeHeaderValue(ApplicationJson);

                    //Post async, do not care about holding context, consumer of this method shoul hold context
                    HttpResponseMessage response = await client.PostAsync(postUrl, content).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;

                    }
                    else
                    {
                        base.Logger.Error("Error in Service Call for url " + postUrl + "  ReasonPhrase : " + response.ReasonPhrase);

                    }
                }
            }
            catch (Exception ex)
            {
                base.Logger.Error("Exception in Service Call : " + postUrl, ex);
            }
            return false;
        }


        public async Task<R> PostJsonAsync<T, R>(string postUrl, T payload)
        {
            base.Logger.Info("PostJsonAsync<T, R>: postUrl" + postUrl);

            R responseObject = default(R);
            try
            {
                using (HttpClient client = this.GetHttpClient(postUrl))
                {


                    string stringPayload = JsonConvert.SerializeObject(payload);
                    // HTTP POST
                    StringContent content = new StringContent(stringPayload);

                    content.Headers.ContentType = new MediaTypeHeaderValue(ApplicationJson);

                    //Post async, do not care about holding context, consumer of this method shoul hold context
                    HttpResponseMessage response = await client.PostAsync(postUrl, content).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        //read response
                        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        responseObject = JsonConvert.DeserializeObject<R>(result);
                    }
                    else
                    {
                        base.Logger.Error("Error in Service Call for url " + postUrl + "  ReasonPhrase : " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                base.Logger.Error("Exception in Service Call : " + postUrl, ex);
            }
            return responseObject;
        }

        #endregion

        /// <summary>
        /// Send Stream
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="postUrl"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task<R> PostHttpAsync<R>(string postUrl, HttpContent payload)
        {
            base.Logger.Info("PostHttpAsync<R>: postUrl" + postUrl);

            R responseObject = default(R);
            try
            {
                using (HttpClient client = this.GetHttpClient(postUrl))
                {


                    //Post async, do not care about holding context, consumer of this method shoul hold context
                    HttpResponseMessage response = await client.PostAsync(postUrl, payload).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        //read response
                        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        responseObject = JsonConvert.DeserializeObject<R>(result);
                    }
                    else
                    {
                        base.Logger.Error("Error in Service Call for url " + postUrl + "  ReasonPhrase : " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                base.Logger.Error("Exception in Service Call : " + postUrl, ex);
            }
            return responseObject;
        }

        /// <summary>
        ///  Send Stream
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task<bool> PostHttpAsync(string postUrl, HttpContent payload)
        {
            base.Logger.Info("PostHttpAsync: postUrl" + postUrl);

            try
            {
                using (HttpClient client = this.GetHttpClient(postUrl))
                {


                    //Post async, do not care about holding context, consumer of this method shoul hold context
                    HttpResponseMessage response = await client.PostAsync(postUrl, payload).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        //read response
                        return true;
                    }
                    else
                    {
                        base.Logger.Error("Error in Service Call for url " + postUrl + "  ReasonPhrase : " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                base.Logger.Error("Exception in Service Call : " + postUrl, ex);
            }
            return false;
        }

        /// <summary>
        /// read stream
        /// </summary>
        /// <param name="resourceUrl"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetSteamAsync(string resourceUrl)
        {
            base.Logger.Info("GetSteamAsync: resourceUrl" + resourceUrl);
            try
            {
                using (HttpClient client = this.GetHttpClient(resourceUrl))
                {

                    HttpResponseMessage response = await client.GetAsync(resourceUrl).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        return response;

                    }
                    else
                    {
                        base.Logger.Error("Error in Service Call for url " + resourceUrl + "  ReasonPhrase : " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                base.Logger.Error("Exception in Service Call : " + resourceUrl, ex);
            }
            return null;
        }



        #region Private Methods

        private HttpClient GetHttpClient(string apiBaseUrl)
        {
            string authType = "NTLM";
            // Create a network credential with username, password, and damain name
            var credential = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["ApiUserName"], System.Configuration.ConfigurationManager.AppSettings["ApiPassword"], System.Configuration.ConfigurationManager.AppSettings["ApiUserDomain"]);
            var userCache = new CredentialCache();

            // Add the target Uri to the CredentialCache with credential object          
            userCache.Add(new Uri(apiBaseUrl), authType, credential);

            HttpClientHandler authtHandler = null;

            if (Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["Environment"]) == "Development")
            {
                authtHandler = new HttpClientHandler()
                {
                    Credentials = CredentialCache.DefaultCredentials
                };
            }
            else
            {
                authtHandler = new HttpClientHandler()
                {
                    Credentials = userCache
                };

            }
            HttpClient client = new HttpClient(authtHandler);

            client.BaseAddress = new Uri(apiBaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJson));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationOctet));
            return client;
        }
        #endregion
    }
}
