using System.Security.Claims;
using DBI.Server.Features.Identity.Models.Entities;
using DBI.Server.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;

namespace DBI.Server.Infrastructure.Authentication;

static class ControllerExtensions
{
    public static async Task<PrincipalEntity?> GetPrincipal(this ControllerContext context, ApplicationDbContext dbContext, CancellationToken cancellationToken = default)
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

        return await dbContext.Principals.FindAsync([principalId], cancellationToken);
    }

    public static async Task<PrincipalEntity> RequirePrincipal(this ControllerContext context, ApplicationDbContext dbContext, CancellationToken cancellationToken = default) =>
        await GetPrincipal(context, dbContext, cancellationToken) ?? throw new InvalidOperationException("Could not determine current principal.");
}
