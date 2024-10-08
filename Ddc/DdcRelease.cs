﻿namespace DBI.Ddc;

/// <summary>
///     Release of the DDC project.
/// </summary>
/// <seealso cref="DdcRelease" />
public class DdcRelease
{
    public required string HtmlUrl { get; init; }
    public required string Name { get; init; }
    public required IReadOnlyCollection<DdcAsset> Assets { get; init; }

    public DdcAsset? Content => Assets.FirstOrDefault(a => a.Name == "data.zip");
}
