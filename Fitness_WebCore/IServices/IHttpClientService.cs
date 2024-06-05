using Fitness_WebCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_WebCore.IServices
{
    public interface IHttpClientService
    {
       
        // Get
        HttpClientResponse<RESPONSE_TYPE> Get<RESPONSE_TYPE>(string requestUri);
        Task<HttpClientResponse<RESPONSE_TYPE>> GetAsync<RESPONSE_TYPE>(string requestUri);

        // Post
        HttpClientResponse<RESPONSE_TYPE> Post<RESPONSE_TYPE, BODY_TYPE>(string requestUri, BODY_TYPE? body);
        Task<HttpClientResponse<RESPONSE_TYPE>> PostAsync<RESPONSE_TYPE, BODY_TYPE>(string requestUri, BODY_TYPE? body);

        // Put
        HttpClientResponse<RESPONSE_TYPE> Put<RESPONSE_TYPE, BODY_TYPE>(string requestUri, BODY_TYPE? body);
        Task<HttpClientResponse<RESPONSE_TYPE>> PutAsync<RESPONSE_TYPE, BODY_TYPE>(string requestUri, BODY_TYPE? body);

        // Patch
        HttpClientResponse<RESPONSE_TYPE> Patch<RESPONSE_TYPE, BODY_TYPE>(string requestUri, BODY_TYPE? body);
        Task<HttpClientResponse<RESPONSE_TYPE>> PatchAsync<RESPONSE_TYPE, BODY_TYPE>(string requestUri, BODY_TYPE? body);

        // Delete
        HttpClientResponse<RESPONSE_TYPE> Delete<RESPONSE_TYPE>(string requestUri);
        Task<HttpClientResponse<RESPONSE_TYPE>> DeleteAsync<RESPONSE_TYPE>(string requestUri);
    }
}
