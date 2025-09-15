using System.ComponentModel.DataAnnotations.Schema;
using EasyExtensions.EntityFrameworkCore.Abstractions;

namespace EasyID.Server.Database.Models
{
    [Table("groups")]
    public class Group : BaseEntity<Guid>
    {
        [Column("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("is_system")]
        public bool IsSystem { get; set; }

        public virtual ICollection<GroupUser> GroupUsers { get; set; } = [];
    }
}
