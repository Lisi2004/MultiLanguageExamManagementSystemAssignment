using System.Globalization;
using LifeEcommerce.Helpers;
using MultiLanguageExamManagementSystem.Services.IServices;
using Microsoft.Extensions.Logging;

namespace MultiLanguageExamManagementSystem.Helpers
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CultureMiddleware> _logger;

        public CultureMiddleware(RequestDelegate next, ILogger<CultureMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                string[] languages = context.Request.Headers["Accept-Language"].ToString().Split(',');
                string defaultLanguage = "en";
                string currentLanguage = languages.FirstOrDefault()?.Trim();

                if (!string.IsNullOrEmpty(currentLanguage))
                {
                    defaultLanguage = currentLanguage;
                }

                CultureInfo cultureInfo = new CultureInfo(defaultLanguage);
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;

                _logger.LogInformation("Current culture set to {Culture}", defaultLanguage);
            }
            catch (CultureNotFoundException ex)
            {
                _logger.LogError(ex, "Culture not found. Setting to default 'en' culture.");
                CultureInfo defaultCulture = new CultureInfo("en");
                CultureInfo.CurrentCulture = defaultCulture;
                CultureInfo.CurrentUICulture = defaultCulture;
            }

            await _next(context);
        }
    }
}
