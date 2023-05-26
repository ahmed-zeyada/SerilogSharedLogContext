using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Serilog.SharedLogContext
{
    internal class SharedLogContextMiddleware
    {
        private readonly ISharedLogContextInitializer _sharedLogContextInitializer;
        private readonly RequestDelegate _next;

        public SharedLogContextMiddleware(RequestDelegate next, ISharedLogContextInitializer sharedLogContextInitializer)
        {
            _next = next;
            _sharedLogContextInitializer = sharedLogContextInitializer;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var sharedLogContext = _sharedLogContextInitializer.BeginContext();

            try
            {
                await _next(httpContext);
            }
            finally
            {
                sharedLogContext.Dispose();
            }
        }
    }
}