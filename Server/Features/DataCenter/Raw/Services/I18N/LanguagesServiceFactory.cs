using System.Text.Json;
using Server.Features.DataCenter.Models.I18N;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Services.Internal;
using Server.Features.DataCenter.Repositories;

namespace Server.Features.DataCenter.Raw.Services.I18N;

public class LanguagesServiceFactory
{
    readonly LanguageServiceFactory _frLanguageFactory;
    readonly LanguageServiceFactory _enLanguageFactory;
    readonly LanguageServiceFactory _esLanguageFactory;
    readonly LanguageServiceFactory _deLanguageFactory;
    readonly LanguageServiceFactory _ptLanguageFactory;

    public LanguagesServiceFactory(IRawDataRepository rawDataRepository)
    {
        _frLanguageFactory = new LanguageServiceFactory(rawDataRepository, RawDataType.I18NFr);
        _enLanguageFactory = new LanguageServiceFactory(rawDataRepository, RawDataType.I18NEn);
        _esLanguageFactory = new LanguageServiceFactory(rawDataRepository, RawDataType.I18NEs);
        _deLanguageFactory = new LanguageServiceFactory(rawDataRepository, RawDataType.I18NDe);
        _ptLanguageFactory = new LanguageServiceFactory(rawDataRepository, RawDataType.I18NPt);
    }

    public async Task<LanguagesService> CreateLanguagesService(string version = "latest", CancellationToken cancellationToken = default) =>
        new()
        {
            French = await _frLanguageFactory.CreateServiceAsync(version, cancellationToken),
            English = await _enLanguageFactory.CreateServiceAsync(version, cancellationToken),
            Spanish = await _esLanguageFactory.CreateServiceAsync(version, cancellationToken),
            German = await _deLanguageFactory.CreateServiceAsync(version, cancellationToken),
            Portuguese = await _ptLanguageFactory.CreateServiceAsync(version, cancellationToken)
        };

    class LanguageServiceFactory : ParsedDataServiceFactory<LanguageService>
    {
        readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public LanguageServiceFactory(IRawDataRepository rawDataRepository, RawDataType dataType) : base(rawDataRepository, dataType)
        {
        }

        protected override async Task<LanguageService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
        {
            string languageCode = DataType switch
            {
                RawDataType.I18NFr => "fr",
                RawDataType.I18NEn => "en",
                RawDataType.I18NEs => "es",
                RawDataType.I18NDe => "de",
                RawDataType.I18NPt => "pt",
                _ => throw new ArgumentOutOfRangeException(nameof(DataType), DataType, null)
            };

            await using Stream stream = file.OpenRead();
            LocalizationTable? data = await JsonSerializer.DeserializeAsync<LocalizationTable>(stream, _jsonSerializerOptions, cancellationToken);
            return data == null ? null : new LanguageService(languageCode, data.Entries);
        }
    }
}
