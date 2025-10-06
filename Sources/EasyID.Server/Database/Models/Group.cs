using EasyID.Server.Database.Models.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyID.Server.Database.Models
{
    [Table("groups")]
    public class Group : BaseIdentityEntity
    {
        public virtual ICollection<GroupUser> GroupUsers { get; set; } = [];
        public virtual ICollection<RoleGroup> RoleGroups { get; set; } = [];
    }
}
