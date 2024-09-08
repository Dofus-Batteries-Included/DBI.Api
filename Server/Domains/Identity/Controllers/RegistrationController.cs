using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Domains.Identity.Models.Entities;
using Server.Infrastructure.Authentication;
using Server.Infrastructure.Database;

namespace Server.Domains.Identity.Controllers;

[Route("identity")]
[ApiController]
public class RegistrationController : ControllerBase
{
    readonly ApplicationDbContext _context;

    public RegistrationController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Register account
    /// </summary>
    [HttpPost("register")]
    public async Task<Guid> Register(long accountId, string accountName)
    {
        Guid token = Guid.NewGuid();
        PrincipalEntity principal = new(accountId, accountName, token);
        await _context.Principals.AddAsync(principal);
        await _context.SaveChangesAsync();
        return token;
    }

    /// <summary>
    ///     Refresh API key
    /// </summary>
    [Authorize]
    [HttpPost("refresh")]
    public async Task<Guid> RefreshApiKey()
    {
        PrincipalEntity principal = await ControllerContext.RequirePrincipal(_context);
        principal.RefreshToken();
        await _context.SaveChangesAsync();
        return principal.Token;
    }

    /// <summary>
    ///     Revoke registration
    /// </summary>
    [Authorize]
    [HttpPost("revoke")]
    public async Task Revoke()
    {
        PrincipalEntity principal = await ControllerContext.RequirePrincipal(_context);
        principal.Revoke();
        await _context.SaveChangesAsync();
    }
}
