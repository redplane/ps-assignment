using System.Net;
using Assignment.Apis.Models.HttpResponses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assignment.Apis.Filters.Exceptions
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        #region Properties

        private readonly ILogger _logger;
        
        #endregion
        
        #region Constructor

        public ValidationExceptionFilter(ILogger<ValidationExceptionFilter> logger)
        {
            _logger = logger;
        }

        #endregion
        
        #region Methods

        public virtual void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
                return;

            if (context.Exception is not ValidationException validationException)
                return;

            _logger.LogDebug(validationException.Message, validationException.Data);
            
            var messages = validationException.Errors.Select(x => x.ErrorMessage).ToArray();
            var badRequestResponse = new BadRequestResponse(messages);
            context.Result = new ObjectResult(badRequestResponse)
            {
                StatusCode = (int) HttpStatusCode.BadRequest
            };
            context.ExceptionHandled = true;
        }

        #endregion
    }
}