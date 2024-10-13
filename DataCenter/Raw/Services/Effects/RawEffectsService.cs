using DBI.DataCenter.Raw.Models.Effects;

namespace DBI.DataCenter.Raw.Services.Effects;

/// <summary>
/// </summary>
public class RawEffectsService(IReadOnlyCollection<RawEffect> effects)
{
    readonly Dictionary<int, RawEffect> _effects = effects.ToDictionary(effect => effect.Id, effect => effect);

    public RawEffect? GetEffect(int effectId) => _effects.GetValueOrDefault(effectId);
    public IEnumerable<RawEffect> GetEffects() => _effects.Values;
}
