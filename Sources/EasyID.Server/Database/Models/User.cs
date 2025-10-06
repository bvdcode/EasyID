using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using EasyExtensions.EntityFrameworkCore.Abstractions;

namespace EasyID.Server.Database.Models
{
    [Table("users")]
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User : BaseEntity<Guid>
    {
        [Column("username")]
        public string Username { get; set; } = null!;

        [Column("email")]
        public string Email { get; set; } = null!;

        [Column("phone_number")]
        public string? PhoneNumber { get; set; } = null!;

        [Column("password_phc")]
        public string PasswordPhc { get; set; } = null!;

        [Column("password_version")]
        public int PasswordVersion { get; set; }

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("middle_name")]
        public string? MiddleName { get; set; }

        [Column("failed_count")]
        public int FailedCount { get; set; }

        [Column("avatar_webp_bytes")]
        public byte[]? AvatarWebPBytes { get; set; }

        public virtual ICollection<GroupUser> GroupUsers { get; set; } = [];
    }
}
