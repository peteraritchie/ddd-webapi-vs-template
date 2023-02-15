using Microsoft.AspNetCore.Mvc;

namespace WebApi.Common;

/// <summary>
/// 
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "In progress.")]
public static class ProblemDetailsExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_"></param>
    /// <returns></returns>
    public static string GetContentType(this ProblemDetails _)
    {
        return ModernMediaTypeNames.Application.ProblemJson;
    }
}
