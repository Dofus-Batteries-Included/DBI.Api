namespace Server.Infrastructure.Authentication;

static class ApiKeyAuthentication
{
    public const string Header = "Authorization";
    public const string Scheme = "ApiKey";
    public const string SubClaim = "sub";
    public const string NameClaim = "name";
}
