using System.Net;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using EasyExtensions.EntityFrameworkCore.Abstractions;

namespace EasyID.Server.Database.Models
{
    [Table("refresh_tokens")]
    [Index(nameof(Token), IsUnique = true)]
    public class RefreshToken : BaseEntity<Guid>
    {
        [Column("token")]
        public string Token { get; set; } = string.Empty;

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("ip_address")]
        public IPAddress IpAddress { get; set; } = IPAddress.None;

        [Column("country")]
        public string Country { get; set; } = string.Empty;

        [Column("city")]
        public string City { get; set; } = string.Empty;

        [Column("user_agent")]
        public string UserAgent { get; set; } = string.Empty;

        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [Column("revoked_at")]
        public DateTime? RevokedAt { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
