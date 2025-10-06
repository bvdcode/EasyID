using EasyID.Server.Database.Models.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyID.Server.Database.Models
{
    [Table("roles")]
    public class Role : BaseIdentityEntity
    {
        public virtual ICollection<RoleGroup> RoleGroups { get; set; } = [];
        public virtual ICollection<PermissionRole> PermissionRoles { get; set; } = [];
    }
}
