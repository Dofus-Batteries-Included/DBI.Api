using Microsoft.AspNetCore.Mvc;
using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Repositories;

namespace Server.Domains.DataCenter.Controllers;

/// <summary>
///     Retrieve raw data in JSON files.
/// </summary>
[Route("data-center/versions/{gameVersion}/raw")]
[Tags("Raw data")]
[ApiController]
public class RawDataController : ControllerBase
{
    readonly IRawDataRepository _repository;

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
    public async Task<FileResult> GetMapPositions(string gameVersion = "latest")
    {
        IRawDataFile file = await _repository.GetRawDataFileAsync(gameVersion, RawDataType.MapPositions);
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
    public async Task<FileResult> GetPointsOfInterest(string gameVersion = "latest")
    {
        IRawDataFile file = await _repository.GetRawDataFileAsync(gameVersion, RawDataType.PointOfInterest);
        return File(file.OpenRead(), file.ContentType, file.Name);
    }
}
