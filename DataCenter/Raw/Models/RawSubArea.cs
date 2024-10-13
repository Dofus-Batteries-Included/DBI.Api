namespace DBI.DataCenter.Raw.Models;

/// <summary>
/// </summary>
public class RawSubArea
{
    public int Id { get; set; }
    public int NameId { get; set; }
    public int AreaId { get; set; }
    public int WorldMapId { get; set; }
    public uint Level { get; set; }
    public RawPosition Center { get; set; }
    public RawBounds Bounds { get; set; } = new();
    public IReadOnlyCollection<int> Neighbours { get; set; } = [];
    public IReadOnlyCollection<int> EntranceMapIds { get; set; } = [];
    public IReadOnlyCollection<int> ExitMapIds { get; set; } = [];
    public int? ZaapMapId { get; set; }
    public bool Capturable { get; set; }
    public bool BasicAccountAllowed { get; set; }
    public bool PsiAllowed { get; set; }
    public bool MountAutoTripAllowed { get; set; }
    public bool IsConquestVillage { get; set; }
    public bool DisplayOnWorldMap { get; set; }
    public IReadOnlyCollection<uint> CustomWorldMap { get; set; } = [];
}
