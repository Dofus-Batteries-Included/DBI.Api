using NSwag;
using NSwag.Generation.Processors.Security;
using Server.Domains.TreasureSolver.Services;
using Server.Domains.TreasureSolver.Services.Clues;
using Server.Domains.TreasureSolver.Services.Clues.DataSources;
using Server.Domains.TreasureSolver.Services.I18N;
using Server.Domains.TreasureSolver.Services.Maps;
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
        services.AddSingleton<MapsService>();
        services.AddSingleton<IMapsService, MapsService>(s => s.GetRequiredService<MapsService>());
        services.AddSingleton<CluesService>();
        services.AddSingleton<ICluesService, CluesService>(s => s.GetRequiredService<CluesService>());
        services.AddSingleton<LanguagesService>();

        services.AddHostedService<RefreshDplnDataSource>();
        services.AddHostedService<RefreshGameData>();
        
        services.AddOpenApiDocument(
            settings =>
            {
                settings.DocumentName = "treasure-solver";
                settings.Title = "Treasure Solver - API";
                settings.Description = "Dofus Treasure Hunt solver using data collected by the players.";
                settings.Version = Metadata.Version?.ToString() ?? "~dev";

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
