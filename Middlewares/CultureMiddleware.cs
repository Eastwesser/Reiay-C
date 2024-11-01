using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Relay.Middlewares
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestCultureProvider _cultureProvider;

        public CultureMiddleware(RequestDelegate next, RequestCultureProvider cultureProvider)
        {
            _next = next;
            _cultureProvider = cultureProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var culture = _cultureProvider.DetermineRequestCulture(context);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            await _next(context);
        }
    }
}
