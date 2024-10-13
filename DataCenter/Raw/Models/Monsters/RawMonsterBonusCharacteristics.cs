namespace DBI.DataCenter.Raw.Models.Monsters;

public class RawMonsterBonusCharacteristics
{
    public int LifePoints { get; set; }
    public ushort Strength { get; set; }
    public ushort Wisdom { get; set; }
    public ushort Chance { get; set; }
    public ushort Agility { get; set; }
    public ushort Intelligence { get; set; }
    public short EarthResistance { get; set; }
    public short AirResistance { get; set; }
    public short FireResistance { get; set; }
    public short WaterResistance { get; set; }
    public short NeutralResistance { get; set; }
    public byte TackleEvade { get; set; }
    public byte TackleBlock { get; set; }
    public byte BonusEarthDamage { get; set; }
    public byte BonusAirDamage { get; set; }
    public byte BonusFireDamage { get; set; }
    public byte BonusWaterDamage { get; set; }
    public byte ApRemoval { get; set; }
}
