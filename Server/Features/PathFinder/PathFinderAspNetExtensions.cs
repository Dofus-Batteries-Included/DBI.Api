using NSwag;
using NSwag.Generation.Processors.Security;
using Server.Common.OpenApi;
using Server.Infrastructure.Authentication;

namespace Server.Features.PathFinder;

static class PathFinderAspNetExtensions
{
    public static void ConfigurePathFinder(this IServiceCollection services) =>
        services.AddOpenApiDocument(
            settings =>
            {
                settings.DocumentName = "path-finder";
                settings.Title = "Path Finder - API";
                settings.Description = "Dofus path finder using data extracted from the game.";
                settings.Version = Metadata.Version?.ToString() ?? "~dev";
                settings.OperationProcessors.Insert(0, new FilterOperationsByRoutePrefix("/path-finder"));

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
