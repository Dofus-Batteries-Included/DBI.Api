using DBI.DataCenter.Structured.Models.Items;
using DBI.DataCenter.Structured.Services.Items;
using DBI.Server.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.DataCenter.Controllers;

/// <summary>
///     Item types endpoints
/// </summary>
[Route("data-center/versions/{gameVersion}/items/types")]
[OpenApiTag("Items - Types")]
[ApiController]
public class ItemTypesController(ItemServicesFactory itemServicesFactory) : ControllerBase
{
    /// <summary>
    ///     Get item types
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<ItemType>> GetItemTypes(string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        ItemTypesService itemTypesService = await itemServicesFactory.CreateItemTypesService(gameVersion, cancellationToken);
        return itemTypesService.GetItemTypes() ?? throw new NotFoundException($"Could not find item types in version {gameVersion}.");
    }

    /// <summary>
    ///     Get item type
    /// </summary>
    [HttpGet("{itemTypeId:int}")]
    public async Task<ItemType> GetItemType(int itemTypeId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        ItemTypesService itemTypesService = await itemServicesFactory.CreateItemTypesService(gameVersion, cancellationToken);
        return itemTypesService.GetItemType(itemTypeId) ?? throw new NotFoundException($"Could not find item type in version {gameVersion}.");
    }

    /// <summary>
    ///     Get items in type
    /// </summary>
    [HttpGet("{itemTypeId:int}/items")]
    public async Task<IEnumerable<Item>> GetItemsInType(int itemTypeId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        ItemsService itemsService = await itemServicesFactory.CreateItemsService(gameVersion, cancellationToken);
        return itemsService.GetItemsInType(itemTypeId) ?? throw new NotFoundException($"Could not find items in version {gameVersion}.");
    }
}
