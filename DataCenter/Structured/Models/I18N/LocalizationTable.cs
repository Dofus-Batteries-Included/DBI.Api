namespace DBI.DataCenter.Structured.Models.I18N;

class LocalizationTable
{
    public string LanguageCode { get; init; } = string.Empty;
    public Dictionary<int, string> Entries { get; init; } = new();
}
