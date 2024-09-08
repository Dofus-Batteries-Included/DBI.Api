using Server.Domains.TreasureSolver.Models;

namespace Server.Domains.TreasureSolver.Services.Clues;

public interface ICluesService
{
    event EventHandler DataRefreshed;

    Clue? GetClue(int id);
    IEnumerable<Clue> GetClues();
}
