namespace DBI.DataCenter.Raw.Models.Monsters;

public class RawMonsterDrop
{
    public int DropId { get; set; }
    public int MonsterId { get; set; }
    public int ObjectId { get; set; }
    public float PercentDropForGrade1 { get; set; }
    public float PercentDropForGrade2 { get; set; }
    public float PercentDropForGrade3 { get; set; }
    public float PercentDropForGrade4 { get; set; }
    public float PercentDropForGrade5 { get; set; }
    public int Count { get; set; }
    public string Criteria { get; set; } = "";
    public bool HasCriteria { get; set; }
    public bool HiddenIfInvalidCriteria { get; set; }
}
