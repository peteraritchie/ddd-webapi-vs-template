using Microsoft.AspNetCore.Mvc;
using WebApi.Common;

namespace WebApi
{
    public static class ProblemDetailsExtensions
    {
        public static string GetContentType(this ProblemDetails _)
        {
            return ModernMediaTypeNames.Application.ProblemJson;
        }
    }
}
