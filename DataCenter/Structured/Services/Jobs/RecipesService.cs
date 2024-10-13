using DBI.DataCenter.Raw.Models.Jobs;
using DBI.DataCenter.Raw.Models.Skills;
using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Recipes;
using DBI.DataCenter.Raw.Services.Skills;
using DBI.DataCenter.Structured.Models.Items;
using DBI.DataCenter.Structured.Models.Jobs;
using DBI.DataCenter.Structured.Services.Items;

namespace DBI.DataCenter.Structured.Services.Jobs;

public class RecipesService(
    RawRecipesService? rawRecipesService,
    RawSkillNamesService? rawSkillNamesService,
    JobsService jobsService,
    ItemsService itemsService,
    LanguagesService languagesService
)
{
    public IEnumerable<Recipe>? GetRecipes() => rawRecipesService?.GetRecipes().Select(Cook);
    public IEnumerable<Recipe>? GetRecipesOfJob(int jobId) => rawRecipesService?.GetRecipesOfJob(jobId).Select(Cook);
    public IEnumerable<Recipe>? GetRecipesUsingSkill(int skillId) => rawRecipesService?.GetRecipesUsingSkill(skillId).Select(Cook);
    public IEnumerable<Recipe>? GetRecipesWithResult(int resultId) => rawRecipesService?.GetRecipesWithResult(resultId).Select(Cook);
    public IEnumerable<Recipe>? GetRecipesWithResultOfType(int resultTypeId) => rawRecipesService?.GetRecipesWithResultOfType(resultTypeId).Select(Cook);
    public IEnumerable<Recipe>? GetRecipesUsingIngredients(params int[] ingredientIds) => rawRecipesService?.GetRecipesUsingIngredients(ingredientIds).Select(Cook);

    Recipe Cook(RawRecipe recipe)
    {
        List<RecipeIngredient> ingredients = new();
        for (int i = 0; i < recipe.IngredientIds.Count; i++)
        {
            Item? item = itemsService.GetItem(recipe.IngredientIds[i]);
            uint count = recipe.Quantities.ElementAtOrDefault(i);
            ingredients.Add(new RecipeIngredient { Item = item, Count = count });
        }

        RawSkillName? rawSkillName = rawSkillNamesService?.GetSkillName(recipe.SkillId);

        return new Recipe
        {
            Job = jobsService.GetJob(recipe.JobId) ?? new Job { Id = recipe.JobId },
            SkillName = rawSkillName == null ? null : languagesService.Get(rawSkillName.NameId),
            Ingredients = ingredients,
            Result = itemsService.GetItem(recipe.ResultId) ?? new Item { Id = recipe.ResultId }
        };
    }
}
