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
    public class ExceptionMiddlewares
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddlewares(RequestDelegate next)
        {
            _next = next;
        }

        // IMyScopedService is injected into Invoke
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var errors = new Dictionary<string, string[]>
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

                var result = new ValidationProblemDetails(errors)
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
