using System.Security.Claims;
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

        public virtual ICollection<GroupUser> UserGroups { get; set; } = [];

        public ClaimsPrincipal GetClaims()
        {
            ClaimsPrincipal claims = new();
            claims.AddIdentity(new ClaimsIdentity(
            [
                new Claim("sub", Id.ToString()),
                new Claim("username", Username),
                new Claim("email", Email),
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Name, Username),
                new Claim(ClaimTypes.Email, Email),
            ], "EasyID"));
            return claims;
        }
    }
}
