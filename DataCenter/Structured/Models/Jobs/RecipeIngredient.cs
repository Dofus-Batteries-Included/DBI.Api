using DBI.DataCenter.Structured.Models.Items;

namespace DBI.DataCenter.Structured.Models.Jobs;

/// <summary>
///     An ingredient in a recipe.
/// </summary>
public class RecipeIngredient
{
    /// <summary>
    ///     The item in the recipe.
    /// </summary>
    public ItemMinimal? Item { get; set; }

    /// <summary>
    ///     The number of items.
    /// </summary>
    public uint Count { get; set; }
}
