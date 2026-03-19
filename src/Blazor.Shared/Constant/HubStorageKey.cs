namespace Blazor.Shared.Constant;

public static class HubStorageKey
{
    public static class Feature
    {
        public const string FeatureFlag = "FeatureFlag";
    }

    public static class Identity
    {
        public const string AuthToken = "Identity.AuthToken";
        public const string AuthRefreshToken = "Identity.AuthRefreshToken";
    }
}