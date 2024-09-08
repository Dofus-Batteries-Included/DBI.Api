using Server.Domains.TreasureSolver.Models;

namespace Server.Domains.TreasureSolver.Services.I18N;

public class LanguagesService
{
    public LanguageService French { get; } = new("fr");
    public LanguageService English { get; } = new("en");
    public LanguageService Spanish { get; } = new("es");
    public LanguageService German { get; } = new("de");
    public LanguageService Portuguese { get; } = new("pt");
}

public class LanguageService
{
    IReadOnlyDictionary<int, string> _table = new Dictionary<int, string>();

    public LanguageService(string language)
    {
        Language = language;
    }

    public string Language { get; }
    public string? Get(int id) => _table.GetValueOrDefault(id);
    public void SetTable(IReadOnlyDictionary<int, string> table) => _table = table;
}

public static class LanguagesServiceExtensions
{
    public static LocalizedText Get(this LanguagesService service, int id) =>
        new()
        {
            French = service.French.Get(id),
            English = service.English.Get(id),
            Spanish = service.Spanish.Get(id),
            German = service.German.Get(id),
            Portuguese = service.Portuguese.Get(id)
        };
}
