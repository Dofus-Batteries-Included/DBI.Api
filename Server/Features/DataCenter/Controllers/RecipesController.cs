using DBI.DataCenter.Structured.Models.Jobs;
using DBI.DataCenter.Structured.Services.Jobs;
using DBI.Server.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.DataCenter.Controllers;

/// <summary>
///     Recipes endpoints
/// </summary>
[Route("data-center/versions/{gameVersion}/recipes")]
[OpenApiTag("Recipes")]
[ApiController]
public class RecipesController : ControllerBase
{
    readonly JobServicesFactory _jobServicesFactory;

    /// <summary>
    /// </summary>
    public RecipesController(JobServicesFactory jobServicesFactory)
    {
        _jobServicesFactory = jobServicesFactory;
    }

    /// <summary>
    ///     Get recipes
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<Recipe>> GetRecipes(string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        RecipesService recipesService = await _jobServicesFactory.CreateRecipesServiceAsync(gameVersion, cancellationToken);
        return recipesService.GetRecipes() ?? throw new NotFoundException($"Could not find recipes in version {gameVersion}.");
    }

    /// <summary>
    ///     Get recipes of job
    /// </summary>
    [HttpGet("jobs/{jobId:int}")]
    public async Task<IEnumerable<Recipe>> GetRecipesOfJob(int jobId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        RecipesService recipesService = await _jobServicesFactory.CreateRecipesServiceAsync(gameVersion, cancellationToken);
        return recipesService.GetRecipesOfJob(jobId) ?? throw new NotFoundException($"Could not find recipe in version {gameVersion}.");
    }

    /// <summary>
    ///     Get recipes using skill
    /// </summary>
    [HttpGet("skill/{skillId:int}")]
    public async Task<IEnumerable<Recipe>> GetRecipesUsingSkill(int skillId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        RecipesService recipesService = await _jobServicesFactory.CreateRecipesServiceAsync(gameVersion, cancellationToken);
        return recipesService.GetRecipesUsingSkill(skillId) ?? throw new NotFoundException($"Could not find recipe in version {gameVersion}.");
    }

    /// <summary>
    ///     Get recipes with result
    /// </summary>
    [HttpGet("result/{resultId:int}")]
    public async Task<IEnumerable<Recipe>> GetRecipesWithResult(int resultId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        RecipesService recipesService = await _jobServicesFactory.CreateRecipesServiceAsync(gameVersion, cancellationToken);
        return recipesService.GetRecipesWithResult(resultId) ?? throw new NotFoundException($"Could not find recipe in version {gameVersion}.");
    }

    /// <summary>
    ///     Get recipes with result of type
    /// </summary>
    [HttpGet("result/item-type/{resultTypeId:int}")]
    public async Task<IEnumerable<Recipe>> GetRecipesWithResultOfType(int resultTypeId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        RecipesService recipesService = await _jobServicesFactory.CreateRecipesServiceAsync(gameVersion, cancellationToken);
        return recipesService.GetRecipesWithResultOfType(resultTypeId) ?? throw new NotFoundException($"Could not find recipe in version {gameVersion}.");
    }

    /// <summary>
    ///     Get recipes using ingredient (1)
    /// </summary>
    [HttpGet("ingredients/{ingredientId:int}")]
    public async Task<IEnumerable<Recipe>> GetRecipesUsingIngredients1(int ingredientId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        RecipesService recipesService = await _jobServicesFactory.CreateRecipesServiceAsync(gameVersion, cancellationToken);
        return recipesService.GetRecipesUsingIngredients(ingredientId) ?? throw new NotFoundException($"Could not find recipe in version {gameVersion}.");
    }

    /// <summary>
    ///     Get recipes using ingredient (2)
    /// </summary>
    [HttpGet("ingredients/{ingredient1Id:int}/{ingredient2Id:int}")]
    public async Task<IEnumerable<Recipe>> GetRecipesUsingIngredients2(
        int ingredient1Id,
        int ingredient2Id,
        string gameVersion = "latest",
        CancellationToken cancellationToken = default
    )
    {
        RecipesService recipesService = await _jobServicesFactory.CreateRecipesServiceAsync(gameVersion, cancellationToken);
        return recipesService.GetRecipesUsingIngredients(ingredient1Id, ingredient2Id) ?? throw new NotFoundException($"Could not find recipe in version {gameVersion}.");
    }

    /// <summary>
    ///     Get recipes using ingredient (3)
    /// </summary>
    [HttpGet("ingredients/{ingredient1Id:int}/{ingredient2Id:int}/{ingredient3Id:int}")]
    public async Task<IEnumerable<Recipe>> GetRecipesUsingIngredients3(
        int ingredient1Id,
        int ingredient2Id,
        int ingredient3Id,
        string gameVersion = "latest",
        CancellationToken cancellationToken = default
    )
    {
        RecipesService recipesService = await _jobServicesFactory.CreateRecipesServiceAsync(gameVersion, cancellationToken);
        return recipesService.GetRecipesUsingIngredients(ingredient1Id, ingredient2Id, ingredient3Id)
               ?? throw new NotFoundException($"Could not find recipe in version {gameVersion}.");
    }

    /// <summary>
    ///     Get recipes using ingredient (4)
    /// </summary>
    [HttpGet("ingredients/{ingredient1Id:int}/{ingredient2Id:int}/{ingredient3Id:int}/{ingredient4Id:int}")]
    public async Task<IEnumerable<Recipe>> GetRecipesUsingIngredients3(
        int ingredient1Id,
        int ingredient2Id,
        int ingredient3Id,
        int ingredient4Id,
        string gameVersion = "latest",
        CancellationToken cancellationToken = default
    )
    {
        RecipesService recipesService = await _jobServicesFactory.CreateRecipesServiceAsync(gameVersion, cancellationToken);
        return recipesService.GetRecipesUsingIngredients(ingredient1Id, ingredient2Id, ingredient3Id, ingredient4Id)
               ?? throw new NotFoundException($"Could not find recipe in version {gameVersion}.");
    }

    /// <summary>
    ///     Get recipes using ingredient (5)
    /// </summary>
    [HttpGet("ingredients/{ingredient1Id:int}/{ingredient2Id:int}/{ingredient3Id:int}/{ingredient4Id:int}/{ingredient5Id:int}")]
    public async Task<IEnumerable<Recipe>> GetRecipesUsingIngredients3(
        int ingredient1Id,
        int ingredient2Id,
        int ingredient3Id,
        int ingredient4Id,
        int ingredient5Id,
        string gameVersion = "latest",
        CancellationToken cancellationToken = default
    )
    {
        RecipesService recipesService = await _jobServicesFactory.CreateRecipesServiceAsync(gameVersion, cancellationToken);
        return recipesService.GetRecipesUsingIngredients(ingredient1Id, ingredient2Id, ingredient3Id, ingredient4Id, ingredient5Id)
               ?? throw new NotFoundException($"Could not find recipe in version {gameVersion}.");
    }

    /// <summary>
    ///     Get recipes using ingredient (6)
    /// </summary>
    [HttpGet("ingredients/{ingredient1Id:int}/{ingredient2Id:int}/{ingredient3Id:int}/{ingredient4Id:int}/{ingredient5Id:int}/{ingredient6Id:int}")]
    public async Task<IEnumerable<Recipe>> GetRecipesUsingIngredients3(
        int ingredient1Id,
        int ingredient2Id,
        int ingredient3Id,
        int ingredient4Id,
        int ingredient5Id,
        int ingredient6Id,
        string gameVersion = "latest",
        CancellationToken cancellationToken = default
    )
    {
        RecipesService recipesService = await _jobServicesFactory.CreateRecipesServiceAsync(gameVersion, cancellationToken);
        return recipesService.GetRecipesUsingIngredients(ingredient1Id, ingredient2Id, ingredient3Id, ingredient4Id, ingredient5Id, ingredient6Id)
               ?? throw new NotFoundException($"Could not find recipe in version {gameVersion}.");
    }

    /// <summary>
    ///     Get recipes using ingredient (7)
    /// </summary>
    [HttpGet("ingredients/{ingredient1Id:int}/{ingredient2Id:int}/{ingredient3Id:int}/{ingredient4Id:int}/{ingredient5Id:int}/{ingredient6Id:int}/{ingredient7Id:int}")]
    public async Task<IEnumerable<Recipe>> GetRecipesUsingIngredients3(
        int ingredient1Id,
        int ingredient2Id,
        int ingredient3Id,
        int ingredient4Id,
        int ingredient5Id,
        int ingredient6Id,
        int ingredient7Id,
        string gameVersion = "latest",
        CancellationToken cancellationToken = default
    )
    {
        RecipesService recipesService = await _jobServicesFactory.CreateRecipesServiceAsync(gameVersion, cancellationToken);
        return recipesService.GetRecipesUsingIngredients(ingredient1Id, ingredient2Id, ingredient3Id, ingredient4Id, ingredient5Id, ingredient6Id, ingredient7Id)
               ?? throw new NotFoundException($"Could not find recipe in version {gameVersion}.");
    }

    /// <summary>
    ///     Get recipes using ingredient (8)
    /// </summary>
    [HttpGet(
        "ingredients/{ingredient1Id:int}/{ingredient2Id:int}/{ingredient3Id:int}/{ingredient4Id:int}/{ingredient5Id:int}/{ingredient6Id:int}/{ingredient7Id:int}/{ingredient8Id:int}"
    )]
    public async Task<IEnumerable<Recipe>> GetRecipesUsingIngredients3(
        int ingredient1Id,
        int ingredient2Id,
        int ingredient3Id,
        int ingredient4Id,
        int ingredient5Id,
        int ingredient6Id,
        int ingredient7Id,
        int ingredient8Id,
        string gameVersion = "latest",
        CancellationToken cancellationToken = default
    )
    {
        RecipesService recipesService = await _jobServicesFactory.CreateRecipesServiceAsync(gameVersion, cancellationToken);
        return recipesService.GetRecipesUsingIngredients(ingredient1Id, ingredient2Id, ingredient3Id, ingredient4Id, ingredient5Id, ingredient6Id, ingredient7Id, ingredient8Id)
               ?? throw new NotFoundException($"Could not find recipe in version {gameVersion}.");
    }
}
