using DBI.DataCenter.Structured.Models.I18N;

namespace Test.PathFinder.Fake;

public static class FakeLocalizedText
{
    public static LocalizedText Create(string prefix = "TEXT") =>
        new()
        {
            French = prefix + "_FR",
            English = prefix + "_EN",
            Spanish = prefix + "_ES",
            German = prefix + "_DE",
            Portuguese = prefix + "_PT"
        };
}
