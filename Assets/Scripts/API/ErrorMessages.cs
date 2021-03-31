using System.Net;
using System.Collections.Generic;


namespace API
{
    public class ErrorMessage
    {
        private HttpStatusCode _status;
        private APICallFunction _errorType;
        private static readonly Dictionary<APICallFunction, string> Errors = new Dictionary<APICallFunction, string>()
        {
            [APICallFunction.Login] = $"The username or password is invalid.",
            [APICallFunction.Register] = $"The username or email are already in use.",
            [APICallFunction.ServerList] = $"We were unable to retrieve the server list.",
            [APICallFunction.FetchStats] = $"We were unable to contact the statistics server.",
            [APICallFunction.UpdateStats] = $"Failed to update player stats.",
            [APICallFunction.NetworkError] = $"There has been a network error, please check your internet connection.",
        };

        public bool IsOk { get => (Status == HttpStatusCode.OK); }

        public HttpStatusCode Status { get => _status; private set { _status = value; }}
        public ErrorMessage(APICallFunction aAPICallFunction, HttpStatusCode aCode)
        {
            _errorType = aAPICallFunction;
            _status = aCode;

        }
        public override string ToString( )
        {
            if (_status.Equals(HttpStatusCode.OK))
                return "API call successfuly terminated.";
            if (_errorType.Equals(APICallFunction.None))
                return $"An error happened. Returned with code {(int)_status}.";
            return $"API call ended with error code {(int)_status} : \n{Errors[_errorType]}";
        }
    }
}
