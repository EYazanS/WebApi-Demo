using Microsoft.AspNetCore.Http;

using System;
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
                await httpContext.Response.WriteAsync(ex.Message);
            }
        }
    }
}
