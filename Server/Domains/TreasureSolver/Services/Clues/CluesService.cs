using Server.Domains.TreasureSolver.Models;

namespace Server.Domains.TreasureSolver.Services.Clues;

public class CluesService : ICluesService
{
    IReadOnlyDictionary<int, Clue> _clues = new Dictionary<int, Clue>();
    public event EventHandler? DataRefreshed;

    public Clue? GetClue(int id) => _clues.GetValueOrDefault(id);
    public IEnumerable<Clue> GetClues() => _clues.Values;

    public void SaveClues(IReadOnlyDictionary<int, Clue> clues)
    {
        _clues = clues;
        DataRefreshed?.Invoke(this, EventArgs.Empty);
    }
}
