using System.ComponentModel.DataAnnotations.Schema;
using EasyExtensions.EntityFrameworkCore.Abstractions;

namespace EasyID.Server.Database.Models
{
    [Table("permission_roles")]
    public class PermissionRole : BaseEntity<Guid>
    {
        [Column("permission_id")]
        public Guid PermissionId { get; set; }

        [Column("role_id")]
        public Guid RoleId { get; set; }

        public virtual Permission Permission { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
