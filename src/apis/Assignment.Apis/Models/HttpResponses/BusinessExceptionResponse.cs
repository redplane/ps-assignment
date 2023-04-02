using System.Net;

namespace Assignment.Apis.Models.HttpResponses
{
    public class BusinessExceptionResponse : HttpResponse
    {
        #region Constructor

        public BusinessExceptionResponse(string code, string message = null,
            HttpStatusCode status = HttpStatusCode.InternalServerError)
        {
            Code = code;
            Message = message;
            Status = status;
        }

        #endregion

        #region Properties

        public string Code { get; }

        public string Message { get; }

        public object AdditionalData { get; set; }

        #endregion
    }
}