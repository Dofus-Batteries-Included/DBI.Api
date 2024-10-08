﻿namespace DBI.DataCenter.Raw.Models;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawArea
{
    public int Id { get; init; }
    public int NameId { get; init; }
    public int? WorldMapId { get; init; }
    public int? SuperAreaId { get; init; }
    public RawBounds Bounds { get; init; } = new();
    public bool ContainHouses { get; init; }
    public bool ContainPaddocks { get; init; }
}
