using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Server.Domains.Identity.Models.Entities;
using Server.Infrastructure.Database;

namespace Server.Infrastructure.Authentication;

public static class ControllerExtensions
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
