using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domains.Identity.Models.Entities;

public class PrincipalEntity
{
    public PrincipalEntity(long accountId, string accountName, Guid token)
    {
        RegistrationDate = DateTime.Now.ToUniversalTime();
        AccountId = accountId;
        AccountName = accountName;
        Token = token;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }

    /// <summary>
    ///     The date at which this principal has been registered.
    /// </summary>
    public DateTime RegistrationDate { get; private set; }

    /// <summary>
    ///     The ID of the Ankama account associated with the principal.
    /// </summary>
    public long AccountId { get; private set; }

    /// <summary>
    ///     The name of the Ankama account associated with the principal.
    /// </summary>
    [MaxLength(256)]
    public string AccountName { get; private set; }

    /// <summary>
    ///     The API Key used to authenticate requests from the principal.
    /// </summary>
    public Guid Token { get; private set; }

    /// <summary>
    ///     Is the principal revoked. The API Key of a revoked principal can no longer be used.
    /// </summary>
    public bool Revoked { get; private set; }

    public void RefreshToken() => Token = Guid.NewGuid();
    public void Revoke() => Revoked = true;
}
