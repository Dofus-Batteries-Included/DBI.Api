namespace DBI.DataCenter.Raw.Models.Jobs;

public class RawRecipe
{
    public int JobId { get; set; }
    public int ResultId { get; set; }
    public string ResultNameId { get; set; } = "";
    public uint ResultTypeId { get; set; }
    public int SkillId { get; set; }
    public IReadOnlyList<int> IngredientIds { get; set; } = [];
    public IReadOnlyList<uint> Quantities { get; set; } = [];
    public string ChangeVersion { get; set; } = "";
    public double TooltipExpirationDate { get; set; }
}
