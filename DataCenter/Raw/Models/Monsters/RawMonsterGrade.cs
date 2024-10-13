namespace DBI.DataCenter.Raw.Models.Monsters;

public class RawMonsterGrade
{
    public int Grade { get; set; }
    public int GradeXp { get; set; }
    public ushort Level { get; set; }
    public byte HiddenLevel { get; set; }
    public int Lifepoints { get; set; }
    public short ActionPoints { get; set; }
    public short MovementPoints { get; set; }
    public int Vitality { get; set; }
    public short PaDodge { get; set; }
    public short PmDodge { get; set; }
    public ushort Wisdom { get; set; }
    public short EarthResistance { get; set; }
    public short AirResistance { get; set; }
    public short FireResistance { get; set; }
    public short WaterResistance { get; set; }
    public short NeutralResistance { get; set; }
    public byte DamageReflect { get; set; }
    public ushort Strength { get; set; }
    public ushort Intelligence { get; set; }
    public ushort Chance { get; set; }
    public ushort Agility { get; set; }
    public int StartingSpellId { get; set; }
    public sbyte BonusRange { get; set; }
    public RawMonsterBonusCharacteristics? BonusCharacteristics { get; set; }
}
