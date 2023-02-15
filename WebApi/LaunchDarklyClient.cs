namespace WebApi;

/// <summary>
/// 
/// </summary>
public class LaunchDarklyClient : IFeatureFlagService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="flagName"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public TResult GetFlag<TResult>(string flagName) where TResult: struct
    {
        dynamic? value = default;
        if (flagName == "FeatureFlag1") value = true;
        return value;
    }
}
