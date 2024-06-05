using Fitness_WebCore.Helpers;
using Fitness_WebCore.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_WebCore.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;
        private readonly BaseUrl _publicApi;
        private readonly ILogger<HttpClientService> _logger;

        public HttpClientService(
            ILogger<HttpClientService> logger,
            IHttpContextAccessor httpContextAccessor,
            IIdentityService identityService,
            IOptions<BaseUrl> publicApi)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
            _publicApi = publicApi.Value;
            _logger = logger;
        }

        #region GET

        public HttpClientResponse<RESPONSE_TYPE> Get<RESPONSE_TYPE>(string requestUri)
        {
            HttpClient client = CreateHttpClient();

            // execute the request
            var response = client.GetAsync(requestUri).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to authorize user");
            }
            var content = response.Content.ReadAsStringAsync().Result; // raw content as string

            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Url: {_publicApi.ApiUri}/{requestUri} - StatusCode: {response.StatusCode} - ErrorMessage: {content} ");

            return new HttpClientResponse<RESPONSE_TYPE>(response.IsSuccessStatusCode, response.StatusCode, content);
        }

        public async Task<HttpClientResponse<RESPONSE_TYPE>> GetAsync<RESPONSE_TYPE>(string requestUri)
        {
            HttpClient client = CreateHttpClient();

            // execute the request
            var response = await client.GetAsync(requestUri);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to authorize user");
            }
            var content = await response.Content.ReadAsStringAsync(); // raw content as string
            _logger.LogCritical($"Url: {_publicApi.ApiUri}/{requestUri}");

            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Url: {_publicApi.ApiUri}/{requestUri} - StatusCode: {response.StatusCode} - ErrorMessage: {content} ");
            return new HttpClientResponse<RESPONSE_TYPE>(response.IsSuccessStatusCode, response.StatusCode, content);
        }
        #endregion
        #region POST

        public HttpClientResponse<RESPONSE_TYPE> Post<RESPONSE_TYPE, BODY_TYPE>(string requestUri, BODY_TYPE? body)
        {
            HttpClient client = CreateHttpClient();

            // create body content
            var stringPayload = JsonConvert.SerializeObject(body);
            var postBodyContent = new StringContent(stringPayload, null, "application/json");

            // execute the request
            var response = client.PostAsync(requestUri, postBodyContent).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to authorize user");
            }
            var content = response.Content.ReadAsStringAsync().Result; // raw content as string

            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Url: {_publicApi.ApiUri}/{requestUri} - StatusCode: {response.StatusCode} - ErrorMessage: {content} ");
            return new HttpClientResponse<RESPONSE_TYPE>(response.IsSuccessStatusCode, response.StatusCode, content);
        }

        public async Task<HttpClientResponse<RESPONSE_TYPE>> PostAsync<RESPONSE_TYPE, BODY_TYPE>(string requestUri, BODY_TYPE? body)
        {
            HttpClient client = CreateHttpClient();

            // create body content
            var stringPayload = JsonConvert.SerializeObject(body);
            var postBodyContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            // execute the request
            var response = await client.PostAsync(requestUri, postBodyContent);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to authorize user");
            }
            var content = await response.Content.ReadAsStringAsync(); // raw content as string
            _logger.LogCritical($"Url: {_publicApi.ApiUri}/{requestUri}");


            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Url: {_publicApi.ApiUri}/{requestUri} - StatusCode: {response.StatusCode} - ErrorMessage: {content} ");
            return new HttpClientResponse<RESPONSE_TYPE>(response.IsSuccessStatusCode, response.StatusCode, content);
        }
        #endregion
        #region PUT

        public HttpClientResponse<RESPONSE_TYPE> Put<RESPONSE_TYPE, BODY_TYPE>(string requestUri, BODY_TYPE? body)
        {
            HttpClient client = CreateHttpClient();

            var stringPayload = JsonConvert.SerializeObject(body);
            var postBodyContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            // execute the request
            var response = client.PutAsync(requestUri, postBodyContent).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to authorize user");
            }
            var content = response.Content.ReadAsStringAsync().Result; // raw content as string

            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Url: {_publicApi.ApiUri}/{requestUri} - StatusCode: {response.StatusCode} - ErrorMessage: {content} ");
            return new HttpClientResponse<RESPONSE_TYPE>(response.IsSuccessStatusCode, response.StatusCode, content);
        }

        public async Task<HttpClientResponse<RESPONSE_TYPE>> PutAsync<RESPONSE_TYPE, BODY_TYPE>(string requestUri, BODY_TYPE? body)
        {
            HttpClient client = CreateHttpClient();

            var stringPayload = JsonConvert.SerializeObject(body);
            var postBodyContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            // execute the request
            var response = await client.PutAsync(requestUri, postBodyContent);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to authorize user");
            }
            var content = await response.Content.ReadAsStringAsync(); // raw content as string
            _logger.LogCritical($"Url: {_publicApi.ApiUri}/{requestUri}");


            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Url: {_publicApi.ApiUri}/{requestUri} - StatusCode: {response.StatusCode} - ErrorMessage: {content} ");
            return new HttpClientResponse<RESPONSE_TYPE>(response.IsSuccessStatusCode, response.StatusCode, content);
        }
        #endregion
        #region PATCH

        public HttpClientResponse<RESPONSE_TYPE> Patch<RESPONSE_TYPE, BODY_TYPE>(string requestUri, BODY_TYPE? body)
        {
            HttpClient client = CreateHttpClient();

            var stringPayload = JsonConvert.SerializeObject(body);
            var postBodyContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            // execute the request
            var response = client.PatchAsync(requestUri, postBodyContent).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to authorize user");
            }
            var content = response.Content.ReadAsStringAsync().Result; // raw content as string
            _logger.LogCritical($"Url: {_publicApi.ApiUri}/{requestUri}");


            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Url: {_publicApi.ApiUri}/{requestUri} - StatusCode: {response.StatusCode} - ErrorMessage: {content} ");
            return new HttpClientResponse<RESPONSE_TYPE>(response.IsSuccessStatusCode, response.StatusCode, content);
        }

        public async Task<HttpClientResponse<RESPONSE_TYPE>> PatchAsync<RESPONSE_TYPE, BODY_TYPE>(string requestUri, BODY_TYPE? body)
        {
            HttpClient client = CreateHttpClient();

            var stringPayload = JsonConvert.SerializeObject(body);
            var postBodyContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            // execute the request
            var response = await client.PatchAsync(requestUri, postBodyContent);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to authorize user");
            }
            var content = await response.Content.ReadAsStringAsync(); // raw content as string

            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Url: {_publicApi.ApiUri}/{requestUri} - StatusCode: {response.StatusCode} - ErrorMessage: {content} ");
            return new HttpClientResponse<RESPONSE_TYPE>(response.IsSuccessStatusCode, response.StatusCode, content);
        }
        #endregion
        #region DELETE

        public HttpClientResponse<RESPONSE_TYPE> Delete<RESPONSE_TYPE>(string requestUri)
        {
            HttpClient client = CreateHttpClient();

            // execute the request
            var response = client.DeleteAsync(requestUri).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to authorize user");
            }
            var content = response.Content.ReadAsStringAsync().Result; // raw content as string

            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Url: {_publicApi.ApiUri}/{requestUri} - StatusCode: {response.StatusCode} - ErrorMessage: {content} ");
            return new HttpClientResponse<RESPONSE_TYPE>(response.IsSuccessStatusCode, response.StatusCode, content);
        }

        public async Task<HttpClientResponse<RESPONSE_TYPE>> DeleteAsync<RESPONSE_TYPE>(string requestUri)
        {
            HttpClient client = CreateHttpClient();

            // execute the request
            var response = await client.DeleteAsync(requestUri);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Failed to authorize user");
            }
            var content = await response.Content.ReadAsStringAsync(); // raw content as string
            _logger.LogCritical($"Url: {_publicApi.ApiUri}/{requestUri}");


            if (!response.IsSuccessStatusCode)
                _logger.LogError($"Url: {_publicApi.ApiUri}/{requestUri} - StatusCode: {response.StatusCode} - ErrorMessage: {content} ");
            return new HttpClientResponse<RESPONSE_TYPE>(response.IsSuccessStatusCode, response.StatusCode, content);
        }
        #endregion

        private HttpClient CreateHttpClient()
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri($"{_publicApi.ApiUri}/")
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // if item in Context exists, it means token is refreshed. If so, use it for current request
            string access_token = _httpContextAccessor.HttpContext.Items["access_token"]?.ToString();
            if (access_token is null)
                access_token = _httpContextAccessor.HttpContext.User.FindFirstValue("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

            return client;
        }
    }
}
