using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using EasyExtensions.EntityFrameworkCore.Abstractions;

namespace EasyID.Server.Database.Models
{
    [Table("role_groups")]
    [Index(nameof(RoleId), nameof(GroupId), IsUnique = true)]
    public class RoleGroup : BaseEntity<Guid>
    {
        [Column("role_id")]
        public Guid RoleId { get; set; }

        [Column("group_id")]
        public Guid GroupId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual Group Group { get; set; } = null!;
    }
}
