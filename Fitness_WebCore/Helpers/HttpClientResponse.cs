using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_WebCore.Helpers
{
    public class HttpClientResponse<RESPONSE_TYPE>
    {
        public HttpClientResponse(bool isSuccess, HttpStatusCode statusCode, string? data = "")
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode.ToString();
            if (isSuccess && statusCode == HttpStatusCode.OK && data != "")
                Data = JsonConvert.DeserializeObject<RESPONSE_TYPE>(data);
        }
        public bool IsSuccess { get; set; }
        public string StatusCode { get; set; }
        public RESPONSE_TYPE Data { get; set; }
    }
}
