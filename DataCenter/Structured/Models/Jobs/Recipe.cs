using DBI.DataCenter.Structured.Models.I18N;
using DBI.DataCenter.Structured.Models.Items;

namespace DBI.DataCenter.Structured.Models.Jobs;

/// <summary>
///     A recipe.
/// </summary>
public class Recipe
{
    /// <summary>
    ///     The job corresponding to the recipe.
    /// </summary>
    public Job? Job { get; set; }

    /// <summary>
    ///     The skill name.
    /// </summary>
    public LocalizedText? SkillName { get; set; }

    /// <summary>
    ///     The ingredients in the recipe.
    /// </summary>
    public IReadOnlyList<RecipeIngredient> Ingredients { get; set; } = [];

    /// <summary>
    ///     The result of the recipe.
    /// </summary>
    public ItemMinimal? Result { get; set; }
}
