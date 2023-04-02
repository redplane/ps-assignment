using System.Net;
using Assignment.Apis.Models.HttpResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assignment.Apis.Filters.Exceptions
{
    public class ExceptionFilter : IExceptionFilter
    {
        #region Properties

        private readonly ILogger _logger;

        #endregion
        
        #region Constructor

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        #endregion
        
        #region Methods

        public virtual void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
                return;

            var exception = context.Exception;
            _logger.LogError(exception.Message, exception.Data);
            
            var httpFailureResponse = new BusinessExceptionResponse("internal_server_error", context.Exception.Message);
            context.Result = new ObjectResult(httpFailureResponse)
            {
                StatusCode = (int) HttpStatusCode.InternalServerError
            };
        }

        #endregion
    }
}