using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var culture = context.Request.Query["lang"].ToString() ?? "ru";
        CultureInfo.CurrentCulture = new CultureInfo(culture);
        CultureInfo.CurrentUICulture = new CultureInfo(culture);

        await _next(context);
    }
}
