using Business.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionMiddlewares
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public ExceptionMiddlewares(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>
                {
                    { "Errors", new[] {ex.Message } }
                };

                if (ex is InvalidModelException invalidModel)
                {
                    errors = new Dictionary<string, string[]>
                    {
                        { invalidModel.FieldName, new[] {ex.Message } }
                    };
                }

                ValidationProblemDetails result = new ValidationProblemDetails(errors)
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "One or more validation errors occurred."
                };

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(result));
            }
        }
    }
}
