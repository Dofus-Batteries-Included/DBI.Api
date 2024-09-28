﻿namespace Server.Features.DataCenter.Models.I18N;

class LocalizationTable
{
    public string LanguageCode { get; init; } = "";
    public Dictionary<int, string> Entries { get; init; } = new();
}
