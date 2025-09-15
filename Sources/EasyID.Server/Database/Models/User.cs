using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using EasyExtensions.EntityFrameworkCore.Abstractions;
using EasyExtensions.AspNetCore.Authorization.Builders;

namespace EasyID.Server.Database
{
    [Table("users")]
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User : BaseEntity<Guid>
    {
        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("phone_number")]
        public long? PhoneNumber { get; set; }

        [Column("password_phc")]
        public string PasswordPhc { get; set; } = string.Empty;

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

        internal Func<ClaimBuilder, ClaimBuilder>? GetClaims()
        {
            throw new NotImplementedException();
        }
    }
}
