using System.Text.Json;
using Server.Domains.DataCenter.Models.I18N;
using Server.Domains.DataCenter.Models.Raw;
using Server.Domains.DataCenter.Repositories;
using Server.Domains.DataCenter.Services.Internal;

namespace Server.Domains.DataCenter.Services.I18N;

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
            French = await _frLanguageFactory.CreateService(version, cancellationToken),
            English = await _enLanguageFactory.CreateService(version, cancellationToken),
            Spanish = await _esLanguageFactory.CreateService(version, cancellationToken),
            German = await _deLanguageFactory.CreateService(version, cancellationToken),
            Portuguese = await _ptLanguageFactory.CreateService(version, cancellationToken)
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
