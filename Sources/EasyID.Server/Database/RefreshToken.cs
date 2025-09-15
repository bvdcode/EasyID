using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using EasyExtensions.EntityFrameworkCore.Abstractions;

namespace EasyID.Server.Database
{
    [Table("refresh_tokens")]
    [Index(nameof(Token), IsUnique = true)]
    public class RefreshToken : BaseEntity<Guid>
    {
        [Column("token")]
        public string Token { get; set; } = string.Empty;

        [Column("user_id")]
        public Guid UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
