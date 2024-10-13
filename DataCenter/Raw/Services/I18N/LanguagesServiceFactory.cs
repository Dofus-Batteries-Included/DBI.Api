using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.Internal;
using DBI.DataCenter.Structured.Models.I18N;

namespace DBI.DataCenter.Raw.Services.I18N;

/// <summary>
/// </summary>
public class LanguagesServiceFactory
{
    readonly LanguageServiceFactory _frLanguageFactory;
    readonly LanguageServiceFactory _enLanguageFactory;
    readonly LanguageServiceFactory _esLanguageFactory;
    readonly LanguageServiceFactory _deLanguageFactory;
    readonly LanguageServiceFactory _ptLanguageFactory;

    public LanguagesServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    {
        _frLanguageFactory = new LanguageServiceFactory(rawDataRepository, RawDataType.I18NFr, rawDataJsonOptionsProvider);
        _enLanguageFactory = new LanguageServiceFactory(rawDataRepository, RawDataType.I18NEn, rawDataJsonOptionsProvider);
        _esLanguageFactory = new LanguageServiceFactory(rawDataRepository, RawDataType.I18NEs, rawDataJsonOptionsProvider);
        _deLanguageFactory = new LanguageServiceFactory(rawDataRepository, RawDataType.I18NDe, rawDataJsonOptionsProvider);
        _ptLanguageFactory = new LanguageServiceFactory(rawDataRepository, RawDataType.I18NPt, rawDataJsonOptionsProvider);
    }

    public async Task<LanguagesService> CreateLanguagesServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        new()
        {
            French = await _frLanguageFactory.TryCreateServiceAsync(version, cancellationToken),
            English = await _enLanguageFactory.TryCreateServiceAsync(version, cancellationToken),
            Spanish = await _esLanguageFactory.TryCreateServiceAsync(version, cancellationToken),
            German = await _deLanguageFactory.TryCreateServiceAsync(version, cancellationToken),
            Portuguese = await _ptLanguageFactory.TryCreateServiceAsync(version, cancellationToken)
        };

    class LanguageServiceFactory(IRawDataRepository rawDataRepository, RawDataType dataType, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
        : ParsedDataServiceFactory<LanguageService, LocalizationTable>(rawDataRepository, dataType, rawDataJsonOptionsProvider)
    {
        protected override LanguageService? CreateServiceImpl(LocalizationTable data, CancellationToken cancellationToken)
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

            return new LanguageService(languageCode, data.Entries);
        }
    }
}
