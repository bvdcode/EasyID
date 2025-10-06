using System.ComponentModel.DataAnnotations.Schema;
using EasyExtensions.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EasyID.Server.Database.Models
{
    [Table("group_users")]
    [Index(nameof(UserId), nameof(GroupId), IsUnique = true)]
    public class GroupUser : BaseEntity<Guid>
    {
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("group_id")]
        public Guid GroupId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Group Group { get; set; } = null!;
    }
}
