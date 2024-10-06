using System.Text.Json;
using System.Text.Json.Serialization;
using DBI.Server.Common.Exceptions;
using DBI.Server.Features.DataCenter;
using DBI.Server.Features.Identity;
using DBI.Server.Features.PathFinder;
using DBI.Server.Features.TreasureSolver;
using DBI.Server.Infrastructure;
using DBI.Server.Infrastructure.Authentication;
using DBI.Server.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

#if DEBUG
const LogEventLevel defaultLoggingLevel = LogEventLevel.Debug;
#else
const LogEventLevel defaultLoggingLevel = LogEventLevel.Information;
#endif
const LogEventLevel infrastructureLoggingLevel = LogEventLevel.Information;
const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} ({SourceContext}){NewLine}{Exception}";
Log.Logger = new LoggerConfiguration().WriteTo.Console(outputTemplate: outputTemplate).Enrich.WithProperty("SourceContext", "Bootstrap").CreateBootstrapLogger();

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog(
        opt =>
        {
            opt.WriteTo.Console(outputTemplate: outputTemplate)
                .Enrich.WithProperty("SourceContext", "Bootstrap")
                .MinimumLevel.Is(defaultLoggingLevel)
                .MinimumLevel.Override("System.Net.Http.HttpClient", infrastructureLoggingLevel)
                .MinimumLevel.Override("Microsoft.Extensions.Http", infrastructureLoggingLevel)
                .MinimumLevel.Override("Microsoft.AspNetCore", infrastructureLoggingLevel)
                .MinimumLevel.Override("Microsoft.Identity", infrastructureLoggingLevel)
                .MinimumLevel.Override("Microsoft.IdentityModel", infrastructureLoggingLevel)
                .ReadFrom.Configuration(builder.Configuration);
        }
    );

    builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("Application")));

    builder.Services.AddControllers()
        .AddJsonOptions(
            options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            }
        );
    builder.Services.AddControllersWithViews();
    builder.Services.AddResponseCompression();
    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<ExceptionHandler>();
    builder.Services.AddHttpClient();

    builder.Services.AddAuthentication(ApiKeyAuthentication.Scheme).AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthentication.Scheme, opt => { });
    builder.Services.AddAuthorization();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>());

    builder.Services.Configure<RepositoryOptions>(
        o =>
        {
            string? repositoryPath = builder.Configuration.GetValue<string?>("RepositoryPath", null);
            if (!string.IsNullOrWhiteSpace(repositoryPath))
            {
                o.BasePath = Path.GetFullPath(Environment.ExpandEnvironmentVariables(repositoryPath));
            }
        }
    );

    builder.Services.ConfigureIdentity();
    builder.Services.ConfigureDataCenter();
    builder.Services.ConfigureTreasureSolver();
    builder.Services.ConfigurePathFinder();

    WebApplication app = builder.Build();

    ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();

    await EnsureDatabaseExists(app);

    string? pathBase = app.Configuration.GetValue<string>("PathBase");
    if (!string.IsNullOrWhiteSpace(pathBase))
    {
        logger.LogInformation("Using path base '{PathBase}'.", pathBase);
        app.UsePathBase(pathBase);
    }

    app.UseExceptionHandler();

    app.UseOpenApi();
    app.UseSwaggerUi(settings => { settings.PersistAuthorization = true; });

    app.UseAuthorization();
    app.UseResponseCompression();

    app.MapDefaultControllerRoute();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

return;

async Task EnsureDatabaseExists(WebApplication app)
{
    IServiceScopeFactory scopeProvider = app.Services.GetRequiredService<IServiceScopeFactory>();
    using IServiceScope scope = scopeProvider.CreateScope();
    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await context.Database.EnsureCreatedAsync();
}
