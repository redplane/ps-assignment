using Assignment.Apis.Filters.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assignment.Apis.Extensions
{
    public static class ExceptionFilterExtensions
    {
        #region Methods

        public static FilterCollection AddApplicationExceptionFilters(this FilterCollection filters)
        {
            filters.Add<BusinessExceptionFilter>();
            filters.Add<ValidationExceptionFilter>();
            filters.Add<ExceptionFilter>();
            return filters;
        }
        
        #endregion
    }
}