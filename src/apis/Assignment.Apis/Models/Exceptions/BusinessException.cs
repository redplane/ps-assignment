using System.Net;

namespace Assignment.Apis.Models.Exceptions
{
    public class BusinessException : Exception
    {
        #region Constructor

        public BusinessException(HttpStatusCode statusCode, string messageCode, string message = "") : base(message)
        {
            StatusCode = statusCode;
            MessageCode = messageCode;
        }

        public BusinessException(HttpStatusCode statusCode, string messageCode, string message,
            Dictionary<string, object> additionalData) : base(message)
        {
            StatusCode = statusCode;
            MessageCode = messageCode;
            AdditionalData = additionalData;
        }

        #endregion

        #region Properties

        public string MessageCode { get; }

        public HttpStatusCode StatusCode { get; }

        public Dictionary<string, object> AdditionalData { get; }

        #endregion
    }
}