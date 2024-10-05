using System.Security.Claims;
using DBI.Server.Features.Identity.Models.Entities;
using DBI.Server.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;

namespace DBI.Server.Infrastructure.Authentication;

static class ControllerExtensions
{
    public static async Task<PrincipalEntity?> GetPrincipal(this ControllerContext context, ApplicationDbContext dbContext)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        Claim? subClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ApiKeyAuthentication.SubClaim);
        if (subClaim == null || !Guid.TryParse(subClaim.Value, out Guid principalId))
        {
            return null;
        }

        return await dbContext.Principals.FindAsync(principalId);
    }

    public static async Task<PrincipalEntity> RequirePrincipal(this ControllerContext context, ApplicationDbContext dbContext) =>
        await GetPrincipal(context, dbContext) ?? throw new InvalidOperationException("Could not determine current principal.");
}
