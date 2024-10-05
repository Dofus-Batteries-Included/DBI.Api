using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Structured.Models;
using DBI.DataCenter.Structured.Models.I18N;
using DBI.Server.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.DataCenter.Controllers;

/// <summary>
///     Internationalization endpoints
/// </summary>
[Route("data-center/versions/{gameVersion}/i18n")]
[OpenApiTag("I18N")]
[ApiController]
public class I18NController : ControllerBase
{
    readonly LanguagesServiceFactory _languagesServiceFactory;

    /// <summary>
    /// </summary>
    public I18NController(LanguagesServiceFactory languagesServiceFactory)
    {
        _languagesServiceFactory = languagesServiceFactory;
    }

    /// <summary>
    ///     Get text in all languages
    /// </summary>
    [HttpGet("text/{id:int}")]
    public async Task<LocalizedText> GetTextInAllLanguages(int id, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        LanguagesService languagesService = await _languagesServiceFactory.CreateLanguagesServiceAsync(gameVersion, cancellationToken);
        return languagesService.Get(id) ?? throw new NotFoundException("Could not find corresponding text.");
    }

    /// <summary>
    ///     Get text in language
    /// </summary>
    [HttpGet("text/{id:int}/{language}")]
    public async Task<string?> GetTextInLanguage(Language language, int id, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        LanguagesService languagesService = await _languagesServiceFactory.CreateLanguagesServiceAsync(gameVersion, cancellationToken);
        return languagesService.Get(language, id) ?? throw new NotFoundException("Could not find corresponding text.");
    }
}
