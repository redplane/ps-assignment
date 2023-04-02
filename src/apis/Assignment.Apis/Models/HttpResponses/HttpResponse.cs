using System.Net;

namespace Assignment.Apis.Models.HttpResponses
{
    public abstract class HttpResponse
    {
        #region Properties

        public HttpStatusCode Status { get; protected set; }

        #endregion
    }
}