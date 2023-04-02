using Assignment.Apis.Models.Exceptions;
using Assignment.Apis.Models.HttpResponses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assignment.Apis.Filters.Exceptions
{
    public class BusinessExceptionFilter : IExceptionFilter
    {
        #region Properties

        private readonly ILogger<BusinessExceptionFilter> _logger;

        #endregion
        
        #region Constructor

        public BusinessExceptionFilter(ILogger<BusinessExceptionFilter> logger)
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
            if (exception is not BusinessException businessException)
                return;

            _logger.LogError(exception.Message, exception.Data);
            
            var httpFailureResponse = new BusinessExceptionResponse(
                businessException.MessageCode, businessException.Message,
                businessException.StatusCode);
            httpFailureResponse.AdditionalData = businessException.AdditionalData;
            context.Result = new ObjectResult(httpFailureResponse)
            {
                StatusCode = (int) businessException.StatusCode
            };

            context.ExceptionHandled = true;
        }

        #endregion
    }
}