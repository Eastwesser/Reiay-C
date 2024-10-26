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
        var cultureQuery = context.Request.Query["lang"].ToString();
        var culture = !string.IsNullOrEmpty(cultureQuery) && IsValidCulture(cultureQuery)
            ? cultureQuery
            : "ru";

        CultureInfo.CurrentCulture = new CultureInfo(culture);
        CultureInfo.CurrentUICulture = new CultureInfo(culture);

        await _next(context);
    }

    private bool IsValidCulture(string culture)
    {
        try
        {
            _ = new CultureInfo(culture);
            return true;
        }
        catch (CultureNotFoundException)
        {
            return false;
        }
    }
}
