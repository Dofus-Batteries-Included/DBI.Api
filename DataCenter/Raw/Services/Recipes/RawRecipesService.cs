using DBI.DataCenter.Raw.Models.Jobs;

namespace DBI.DataCenter.Raw.Services.Recipes;

/// <summary>
/// </summary>
public class RawRecipesService(IReadOnlyCollection<RawRecipe> recipes)
{
    public IEnumerable<RawRecipe> GetRecipesOfJob(int jobId) => recipes.Where(r => r.JobId == jobId);
    public IEnumerable<RawRecipe> GetRecipesUsingSkill(int skillId) => recipes.Where(r => r.SkillId == skillId);
    public IEnumerable<RawRecipe> GetRecipesWithResult(int resultId) => recipes.Where(r => r.ResultId == resultId);
    public IEnumerable<RawRecipe> GetRecipesWithResultOfType(int resultTypeId) => recipes.Where(r => r.ResultTypeId == resultTypeId);
    public IEnumerable<RawRecipe> GetRecipesUsingIngredients(params int[] ingredientIds) => recipes.Where(r => ingredientIds.All(id => r.IngredientIds.Contains(id)));
    public IEnumerable<RawRecipe> GetRecipes() => recipes;
}
