namespace WebApi;

/// <summary>
/// 
/// </summary>
public interface IFeatureFlagService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="flagName"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    TResult GetFlag<TResult>(string flagName) where TResult: struct;
}
