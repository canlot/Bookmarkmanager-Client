using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bookmark_Manager_Client.DataProvider
{
    public class ErrorResponse
    {
        public string Message { get; set; }
    }

    public class ResponseHandler<T>
    {
        private RestResponse response;
        public T SuccesfullResponseObject { get; private set; }
        public ErrorResponse ErrorResponseObject { get; private set; }

        public ResponseHandler(RestResponse response)
        {
            this.response = response;
        }
        public bool Handle()
        {
            if (response.IsSuccessful)
            {
                SuccesfullResponseObject = JsonSerializer.Deserialize<T>(response.Content);
                return true;
            }
            else
            {
                ErrorResponseObject = JsonSerializer.Deserialize<ErrorResponse>(response.Content);
                return false;
            }
        }
    }
}
