namespace WebApi;

/// <summary>
///     application-specific content types (media types)
/// </summary>
public static class ApplicationContentTypes
{
	private const string vendorTreePrefix = "vnd.";
	private const string applicationTypePrefix = "application/";
	private const string orderSubType = "contoso.sales.order";
	private const string jsonSuffix = "+json";
	private const string charsetParameterName = "charset";
	private const string utf8CharsetArgument = charsetParameterName + "=utf-8";
	private const string versionParameterName = "version";

	/// <summary>
	/// </summary>
	public const string OrderJson = applicationTypePrefix + vendorTreePrefix + orderSubType + jsonSuffix + "; " +
	                                utf8CharsetArgument + "; " + versionParameterName + "=1.0";

	/// <summary>
	/// </summary>
	public const string AcceptedJson = "application/vnd.contoso.accepted+json; charset=utf-8";
}
