using System.Text.Json;
using Server.Common.Exceptions;
using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Models.I18N;
using Server.Domains.DataCenter.Repositories;

namespace Server.Domains.DataCenter.Services.I18N;

public class LanguagesServiceFactory
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    readonly IRawDataRepository _rawDataRepository;

    public LanguagesServiceFactory(IRawDataRepository rawDataRepository)
    {
        _rawDataRepository = rawDataRepository;
    }

    public async Task<LanguagesService> CreateLanguagesService(string version = "latest", CancellationToken cancellationToken = default) =>
        new()
        {
            French = await GetLanguageService(version, Language.Fr, cancellationToken),
            English = await GetLanguageService(version, Language.En, cancellationToken),
            Spanish = await GetLanguageService(version, Language.Es, cancellationToken),
            German = await GetLanguageService(version, Language.De, cancellationToken),
            Portuguese = await GetLanguageService(version, Language.Pt, cancellationToken)
        };

    async Task<LanguageService> GetLanguageService(string version, Language language, CancellationToken cancellationToken = default)
    {
        RawDataType dataType = language switch
        {
            Language.Fr => RawDataType.I18NFr,
            Language.En => RawDataType.I18NEn,
            Language.Es => RawDataType.I18NEs,
            Language.De => RawDataType.I18NDe,
            Language.Pt => RawDataType.I18NPt,
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };

        IRawDataFile file = await _rawDataRepository.GetRawDataFileAsync(version, dataType, cancellationToken);
        await using Stream stream = file.OpenRead();
        LocalizationTable? data = await JsonSerializer.DeserializeAsync<LocalizationTable>(stream, _jsonSerializerOptions, cancellationToken);
        if (data == null)
        {
            throw new BadRequestException($"Could not load language {language} for version {version}");
        }

        return new LanguageService(language.ToString().ToLower(), data.Entries);
    }
}
