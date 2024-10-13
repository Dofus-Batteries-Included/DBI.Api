using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.Jobs;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Recipes;

/// <summary>
/// </summary>
public class RawRecipesServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawRecipesService, RawRecipe[]>(rawDataRepository, RawDataType.Recipes, rawDataJsonOptionsProvider)
{
    protected override RawRecipesService? CreateServiceImpl(RawRecipe[] data, CancellationToken cancellationToken) => new(data);
}
