using EasyID.Server.Database.Models.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyID.Server.Database.Models
{
    [Table("permissions")]
    public class Permission : BaseIdentityEntity
    {
        public virtual ICollection<PermissionRole> PermissionRoles { get; set; } = [];
    }
}
