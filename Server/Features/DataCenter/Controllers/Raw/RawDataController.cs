using Microsoft.AspNetCore.Mvc;
using Server.Features.DataCenter.Models;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Repositories;

namespace Server.Features.DataCenter.Controllers.Raw;

/// <summary>
///     Retrieve raw data in JSON files.
/// </summary>
[Route("data-center/versions/{gameVersion}/raw")]
[Tags("Raw data")]
[ApiController]
public class RawDataController : ControllerBase
{
    readonly IRawDataRepository _repository;

    /// <summary>
    /// </summary>
    public RawDataController(IRawDataRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    ///     Get localization data
    /// </summary>
    /// <remarks>
    ///     Returns a JSON file.
    /// </remarks>
    [HttpGet("i18n/{lang}")]
    [ProducesResponseType<FileResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<FileResult> GetI18N(Language lang, string gameVersion = "latest")
    {
        RawDataType type = lang switch
        {
            Language.Fr => RawDataType.I18NFr,
            Language.En => RawDataType.I18NEn,
            Language.Es => RawDataType.I18NEs,
            Language.De => RawDataType.I18NDe,
            Language.Pt => RawDataType.I18NPt,
            _ => throw new ArgumentOutOfRangeException(nameof(lang), lang, null)
        };

        IRawDataFile file = await _repository.GetRawDataFileAsync(gameVersion, type);
        return File(file.OpenRead(), file.ContentType, file.Name);
    }

    /// <summary>
    ///     Get map positions data
    /// </summary>
    /// <remarks>
    ///     Returns a JSON file.
    /// </remarks>
    [HttpGet("map-positions")]
    [ProducesResponseType<FileResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<FileResult> GetMapPositions(string gameVersion = "latest")
    {
        IRawDataFile file = await _repository.GetRawDataFileAsync(gameVersion, RawDataType.MapPositions);
        return File(file.OpenRead(), file.ContentType, file.Name);
    }

    /// <summary>
    ///     Get map coordinates data
    /// </summary>
    /// <remarks>
    ///     Returns a JSON file.
    /// </remarks>
    [HttpGet("map-coordinates")]
    [ProducesResponseType<FileResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<FileResult> GetMapCoordinates(string gameVersion = "latest")
    {
        IRawDataFile file = await _repository.GetRawDataFileAsync(gameVersion, RawDataType.MapCoordinates);
        return File(file.OpenRead(), file.ContentType, file.Name);
    }

    /// <summary>
    ///     Get points of interest data
    /// </summary>
    /// <remarks>
    ///     Returns a JSON file.
    /// </remarks>
    [HttpGet("points-of-interest")]
    [ProducesResponseType<FileResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<FileResult> GetPointsOfInterest(string gameVersion = "latest")
    {
        IRawDataFile file = await _repository.GetRawDataFileAsync(gameVersion, RawDataType.PointOfInterest);
        return File(file.OpenRead(), file.ContentType, file.Name);
    }

    /// <summary>
    ///     Get world graph data
    /// </summary>
    /// <remarks>
    ///     Returns a JSON file.
    /// </remarks>
    [HttpGet("world-graph")]
    [ProducesResponseType<FileResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<FileResult> GetWorldGraph(string gameVersion = "latest")
    {
        IRawDataFile file = await _repository.GetRawDataFileAsync(gameVersion, RawDataType.WorldGraph);
        return File(file.OpenRead(), file.ContentType, file.Name);
    }

    /// <summary>
    ///     Get super areas data
    /// </summary>
    /// <remarks>
    ///     Returns a JSON file.
    /// </remarks>
    [HttpGet("super-areas")]
    [ProducesResponseType<FileResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<FileResult> GetSuperAreas(string gameVersion = "latest")
    {
        IRawDataFile file = await _repository.GetRawDataFileAsync(gameVersion, RawDataType.SuperAreas);
        return File(file.OpenRead(), file.ContentType, file.Name);
    }

    /// <summary>
    ///     Get areas data
    /// </summary>
    /// <remarks>
    ///     Returns a JSON file.
    /// </remarks>
    [HttpGet("areas")]
    [ProducesResponseType<FileResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<FileResult> GetAreas(string gameVersion = "latest")
    {
        IRawDataFile file = await _repository.GetRawDataFileAsync(gameVersion, RawDataType.Areas);
        return File(file.OpenRead(), file.ContentType, file.Name);
    }

    /// <summary>
    ///     Get sub areas data
    /// </summary>
    /// <remarks>
    ///     Returns a JSON file.
    /// </remarks>
    [HttpGet("sub-areas")]
    [ProducesResponseType<FileResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<FileResult> GetSubAreas(string gameVersion = "latest")
    {
        IRawDataFile file = await _repository.GetRawDataFileAsync(gameVersion, RawDataType.SubAreas);
        return File(file.OpenRead(), file.ContentType, file.Name);
    }

    /// <summary>
    ///     Get maps data
    /// </summary>
    /// <remarks>
    ///     Returns a JSON file.
    /// </remarks>
    [HttpGet("maps")]
    [ProducesResponseType<FileResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<FileResult> GetMaps(string gameVersion = "latest")
    {
        IRawDataFile file = await _repository.GetRawDataFileAsync(gameVersion, RawDataType.Maps);
        return File(file.OpenRead(), file.ContentType, file.Name);
    }

    /// <summary>
    ///     Get world maps data
    /// </summary>
    /// <remarks>
    ///     Returns a JSON file.
    /// </remarks>
    [HttpGet("world-maps")]
    [ProducesResponseType<FileResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<FileResult> GetWorldMaps(string gameVersion = "latest")
    {
        IRawDataFile file = await _repository.GetRawDataFileAsync(gameVersion, RawDataType.WorldMaps);
        return File(file.OpenRead(), file.ContentType, file.Name);
    }
}
