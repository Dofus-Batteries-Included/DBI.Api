using DBI.DataCenter.Structured.Models.Items;
using DBI.DataCenter.Structured.Services.Items;
using DBI.Server.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.DataCenter.Controllers;

/// <summary>
///     Item super types endpoints
/// </summary>
[Route("data-center/versions/{gameVersion}/items/super-types")]
[OpenApiTag("Items - Super types")]
[ApiController]
public class ItemSuperTypesController(ItemServicesFactory itemServicesFactory) : ControllerBase
{
    /// <summary>
    ///     Get super types
    /// </summary>
    [HttpGet]
    public IEnumerable<ItemSuperType> GetItemSuperTypes(string gameVersion = "latest") => Enum.GetValues<ItemSuperType>().Where(i => i != ItemSuperType.None);

    /// <summary>
    ///     Get item types in super type
    /// </summary>
    [HttpGet("{superType}/item-types")]
    public async Task<IEnumerable<ItemType>> GetItemTypesInSuperType(ItemSuperType superType, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        ItemTypesService itemTypesService = await itemServicesFactory.CreateItemTypesService(gameVersion, cancellationToken);
        return itemTypesService.GetItemTypesInSuperType(superType) ?? throw new NotFoundException($"Could not find item types in version {gameVersion}.");
    }

    /// <summary>
    ///     Get items in super type
    /// </summary>
    [HttpGet("{superType}/items")]
    public async Task<IEnumerable<Item>> GetItemsInSuperType(ItemSuperType superType, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        ItemsService itemsService = await itemServicesFactory.CreateItemsService(gameVersion, cancellationToken);
        return itemsService.GetItemsInSuperType(superType) ?? throw new NotFoundException($"Could not find items in version {gameVersion}.");
    }
}
