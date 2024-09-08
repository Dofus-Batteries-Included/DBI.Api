using NSwag;
using NSwag.Generation.Processors.Security;
using Server.Common.OpenApi;
using Server.Domains.TreasureSolver.Services;
using Server.Domains.TreasureSolver.Services.Clues;
using Server.Domains.TreasureSolver.Services.Clues.DataSources;
using Server.Domains.TreasureSolver.Workers;
using Server.Infrastructure.Authentication;

namespace Server.Domains.TreasureSolver;

public static class TreasureSolverAspNetExtensions
{
    public static void ConfigureTreasureSolver(this IServiceCollection services)
    {
        services.AddScoped<IClueRecordsSource, DatabaseClueRecordsSource>();
        services.AddSingleton<StaticCluesDataSourcesService>();
        services.AddScoped<FindCluesService>();
        services.AddScoped<ExportCluesService>();
        services.AddScoped<RegisterCluesService>();
        services.AddScoped<TreasureSolverService>();

        services.AddHostedService<RefreshDplnDataSource>();

        services.AddOpenApiDocument(
            settings =>
            {
                settings.DocumentName = "treasure-solver";
                settings.Title = "Treasure Solver - API";
                settings.Description = "Dofus Treasure Hunt solver using data collected by the players.";
                settings.Version = Metadata.Version?.ToString() ?? "~dev";
                settings.OperationProcessors.Insert(0, new FilterOperationsByRoutePrefix("/treasure-solver"));

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
}
