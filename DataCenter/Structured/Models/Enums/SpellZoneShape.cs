using DBI.DataCenter.Raw.Models.Enums;

namespace DBI.DataCenter.Structured.Models.Enums;

public enum SpellZoneShape : byte
{
    None = 0,
    Empty = 32,
    DiagonalCrossWithoutCenter = 35,
    Star = 42,
    DiagonalCross = 43,
    DiagonalPerpendicularLine = 45,
    DiagonalLine = 47,
    Custom = 59,
    WholeMapWithTheDead = 65,
    Boomerang = 66,
    Circle = 67,
    Checkerboard = 68,
    Fork = 70,
    Square = 71,
    OutsideCircle = 73,
    Line = 76,
    Ring = 79,
    Point = 80,
    CrossWithoutCenter = 81,
    Rectangle = 82,
    PerpendicularLine = 84,
    HalfCircle = 85,
    Cone = 86,
    SquareWithoutDiagonals = 87,
    Cross = 88,
    OutsideComplexCircle = 90,
    WholeMap = 97,
    LineFromCaster = 108
}

static class SpellZoneShapeMappingExtensions
{
    public static SpellZoneShape Cook(this RawSpellZoneShape shape) =>
        shape switch
        {
            RawSpellZoneShape.None => SpellZoneShape.None,
            RawSpellZoneShape.Empty => SpellZoneShape.Empty,
            RawSpellZoneShape.DiagonalCrossWithoutCenter => SpellZoneShape.DiagonalCrossWithoutCenter,
            RawSpellZoneShape.Star => SpellZoneShape.Star,
            RawSpellZoneShape.DiagonalCross => SpellZoneShape.DiagonalCross,
            RawSpellZoneShape.DiagonalPerpendicularLine => SpellZoneShape.DiagonalPerpendicularLine,
            RawSpellZoneShape.DiagonalLine => SpellZoneShape.DiagonalLine,
            RawSpellZoneShape.Custom => SpellZoneShape.Custom,
            RawSpellZoneShape.WholeMapWithTheDead => SpellZoneShape.WholeMapWithTheDead,
            RawSpellZoneShape.Boomerang => SpellZoneShape.Boomerang,
            RawSpellZoneShape.Circle => SpellZoneShape.Circle,
            RawSpellZoneShape.Checkerboard => SpellZoneShape.Checkerboard,
            RawSpellZoneShape.Fork => SpellZoneShape.Fork,
            RawSpellZoneShape.Square => SpellZoneShape.Square,
            RawSpellZoneShape.OutsideCircle => SpellZoneShape.OutsideCircle,
            RawSpellZoneShape.Line => SpellZoneShape.Line,
            RawSpellZoneShape.Ring => SpellZoneShape.Ring,
            RawSpellZoneShape.Point => SpellZoneShape.Point,
            RawSpellZoneShape.CrossWithoutCenter => SpellZoneShape.CrossWithoutCenter,
            RawSpellZoneShape.Rectangle => SpellZoneShape.Rectangle,
            RawSpellZoneShape.PerpendicularLine => SpellZoneShape.PerpendicularLine,
            RawSpellZoneShape.HalfCircle => SpellZoneShape.HalfCircle,
            RawSpellZoneShape.Cone => SpellZoneShape.Cone,
            RawSpellZoneShape.SquareWithoutDiagonals => SpellZoneShape.SquareWithoutDiagonals,
            RawSpellZoneShape.Cross => SpellZoneShape.Cross,
            RawSpellZoneShape.OutsideComplexCircle => SpellZoneShape.OutsideComplexCircle,
            RawSpellZoneShape.WholeMap => SpellZoneShape.WholeMap,
            RawSpellZoneShape.LineFromCaster => SpellZoneShape.LineFromCaster,
            _ => throw new ArgumentOutOfRangeException(nameof(shape), shape, null)
        };
}
