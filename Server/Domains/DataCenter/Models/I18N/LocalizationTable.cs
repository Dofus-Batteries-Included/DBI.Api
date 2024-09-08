namespace Server.Domains.DataCenter.Models.I18N;

public class LocalizationTable
{
    public string LanguageCode { get; init; } = "";
    public Dictionary<int, string> Entries { get; init; } = new();
}
