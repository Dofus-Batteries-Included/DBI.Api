using Server.Common.Models;

namespace Server.Features.DataCenter.Raw.Services.I18N;

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
    public static LocalizedText Get(this LanguagesService service, int id) =>
        new()
        {
            French = service.French?.Get(id),
            English = service.English?.Get(id),
            Spanish = service.Spanish?.Get(id),
            German = service.German?.Get(id),
            Portuguese = service.Portuguese?.Get(id)
        };
}
