using DBI.DataCenter.Structured.Models.Items;
using DBI.DataCenter.Structured.Services.Items;
using DBI.Server.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.DataCenter.Controllers;

/// <summary>
///     Item endpoints
/// </summary>
[Route("data-center/versions/{gameVersion}/items")]
[OpenApiTag("Items")]
[ApiController]
public class ItemsController(ItemServicesFactory itemServicesFactory) : ControllerBase
{
    /// <summary>
    ///     Get items
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<Item>> GetItems(string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        ItemsService itemsService = await itemServicesFactory.CreateItemsService(gameVersion, cancellationToken);
        return itemsService.GetItems() ?? throw new NotFoundException($"Could not find items in version {gameVersion}.");
    }

    /// <summary>
    ///     Get item
    /// </summary>
    [HttpGet("{itemId:int}")]
    public async Task<Item> GetItem(int itemId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        ItemsService itemsService = await itemServicesFactory.CreateItemsService(gameVersion, cancellationToken);
        return itemsService.GetItem(itemId) ?? throw new NotFoundException($"Could not find item in version {gameVersion}.");
    }
}
