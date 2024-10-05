using DBI.Server.Common.OpenApi;
using DBI.Server.Infrastructure.Authentication;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace DBI.Server.Features.Identity;

static class IdentityAspNetExtensions
{
    public static void ConfigureIdentity(this IServiceCollection services) =>
        services.AddOpenApiDocument(
            settings =>
            {
                settings.DocumentName = "identity";
                settings.Title = "Dofus Batteries Included API - Identity";
                settings.Description = "Manage identities.";
                settings.Version = Metadata.Version?.ToString() ?? "~dev";
                settings.OperationProcessors.Insert(0, new FilterOperationsByRoutePrefix("/identity"));
                settings.DocumentProcessors.Add(new SortTagsInDocument());

                const string schemeName = "API key";
                settings.AddSecurity(
                    schemeName,
                    new OpenApiSecurityScheme
                    {
                        Description = "Please enter your API key.",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Name = ApiKeyAuthentication.Header,
                        Type = OpenApiSecuritySchemeType.ApiKey
                    }
                );
                settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor(schemeName));
            }
        );
}
