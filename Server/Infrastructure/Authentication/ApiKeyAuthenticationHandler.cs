﻿using System.Security.Claims;
using System.Text.Encodings.Web;
using DBI.Server.Features.Identity.Models.Entities;
using DBI.Server.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace DBI.Server.Infrastructure.Authentication;

class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    readonly ApplicationDbContext _applicationDbContext;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ApplicationDbContext applicationDbContext
    ) : base(options, logger, encoder)
    {
        _applicationDbContext = applicationDbContext;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyAuthentication.Header, out StringValues apiKeyValues))
        {
            return AuthenticateResult.Fail("Missing API Key");
        }

        string? tokenStr = apiKeyValues.FirstOrDefault();
        if (tokenStr == null || !Guid.TryParse(tokenStr, out Guid token))
        {
            return AuthenticateResult.Fail("Bad API Key");
        }

        PrincipalEntity? principal = await _applicationDbContext.Principals.SingleOrDefaultAsync(p => p.Token == token);
        if (principal == null || principal.Revoked)
        {
            return AuthenticateResult.Fail("Bad API Key");
        }

        List<Claim> claims =
        [
            new Claim(ApiKeyAuthentication.SubClaim, principal.Id.ToString()),
            new Claim(ApiKeyAuthentication.NameClaim, principal.AccountName)
        ];
        ClaimsIdentity identity = new(claims, ApiKeyAuthentication.Scheme, ApiKeyAuthentication.NameClaim, null);
        ClaimsPrincipal claimsPrincipal = new(identity);
        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, ApiKeyAuthentication.Scheme));
    }
}
