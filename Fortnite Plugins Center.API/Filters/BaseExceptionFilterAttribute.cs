using Fortnite_Plugins_Center.Shared.Exceptions;
using Fortnite_Plugins_Center.Shared.Exceptions.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Fortnite_Plugins_Center.API.Filters
{
    public class BaseExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is not BaseException)
            {
                Console.WriteLine($"Unhandled exception: {context.Exception.Message}");

                // Unhandled exception
                context.Exception = new UnhandledErrorException(Guid.NewGuid().ToString());
            }

            var exception = (BaseException)context.Exception;
            context.Result = new JsonResult(exception)
            {
                StatusCode = exception.StatusCode
            };
        }
    }
}
