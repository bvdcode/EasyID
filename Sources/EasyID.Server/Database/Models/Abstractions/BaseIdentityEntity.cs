using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using EasyExtensions.EntityFrameworkCore.Abstractions;

namespace EasyID.Server.Database.Models.Abstractions
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(DisplayName), IsUnique = true)]
    public abstract class BaseIdentityEntity : BaseEntity<Guid>
    {
        [Column("display_name")]
        public string DisplayName { get; set; } = null!;

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        [Column("is_system")]
        public bool IsSystem { get; set; }
    }
}
