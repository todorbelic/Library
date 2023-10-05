using BookStoreClassLibrary.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace BookStoreClassLibrary.Core.Security
{
    public class RefreshToken : IdentityUserToken<Guid>, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string? CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }
        public string? UserAgent { get; set; }
        public string? Device { get; set; }
        public bool IsExpired => DateTime.UtcNow > Expires;
        public bool IsRevoked => Revoked!= null;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
