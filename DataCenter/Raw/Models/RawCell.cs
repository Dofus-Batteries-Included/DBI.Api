﻿namespace DBI.DataCenter.Raw.Models;

/// <summary>
public class RawCell
{
    public int CellNumber { get; init; }
    public int Floor { get; init; }
    public int MoveZone { get; init; }
    public int LinkedZone { get; init; }
    public int Speed { get; init; }
    public bool Los { get; init; }
    public bool Visible { get; init; }
    public bool NonWalkableDuringFight { get; init; }
    public bool NonWalkableDuringRp { get; init; }
    public bool HavenbagCell { get; init; }
}
