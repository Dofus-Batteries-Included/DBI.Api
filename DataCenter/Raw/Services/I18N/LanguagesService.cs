using DBI.DataCenter.Structured.Models;
using DBI.DataCenter.Structured.Models.I18N;

namespace DBI.DataCenter.Raw.Services.I18N;

/// <summary>
/// </summary>
public class LanguagesService
{
    public required LanguageService? French { get; init; }
    public required LanguageService? English { get; init; }
    public required LanguageService? Spanish { get; init; }
    public required LanguageService? German { get; init; }
    public required LanguageService? Portuguese { get; init; }
}

public class LanguageService
{
    readonly Dictionary<int, string> _table;

    public LanguageService(string language, Dictionary<int, string> table)
    {
        Language = language;
        _table = table;
    }

    public string Language { get; }
    public string? Get(int id) => _table.GetValueOrDefault(id);
}

public static class LanguagesServiceExtensions
{
    public static LocalizedText? Get(this LanguagesService service, int id)
    {
        string? french = service.French?.Get(id);
        string? english = service.English?.Get(id);
        string? spanish = service.Spanish?.Get(id);
        string? german = service.German?.Get(id);
        string? portuguese = service.Portuguese?.Get(id);

        if (french == null && english == null && spanish == null && german == null && portuguese == null)
        {
            return null;
        }

        return new LocalizedText
        {
            French = french,
            English = english,
            Spanish = spanish,
            German = german,
            Portuguese = portuguese
        };
    }

    public static string? Get(this LanguagesService service, Language language, int id) =>
        language switch
        {
            Language.Fr => service.French?.Get(id),
            Language.En => service.English?.Get(id),
            Language.Es => service.Spanish?.Get(id),
            Language.De => service.German?.Get(id),
            Language.Pt => service.Portuguese?.Get(id),
            _ => null
        };
}
