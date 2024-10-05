using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace DBI.Server.Common.OpenApi;

class FilterOperationsByRoutePrefix : IOperationProcessor
{
    readonly string _routePrefix;

    public FilterOperationsByRoutePrefix(string routePrefix)
    {
        _routePrefix = routePrefix;
    }

    public bool Process(OperationProcessorContext context) => context.OperationDescription.Path.StartsWith(_routePrefix);
}
