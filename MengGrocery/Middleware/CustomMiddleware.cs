using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using MengGrocery.Helpers;
//using MengGrocery.Log;

namespace MengGrocery.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //do something in the middleware
            var resp = context.Response;
            context.Response.Headers.Add("custom-header", "text");

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

}