using System.ComponentModel.DataAnnotations.Schema;
using EasyExtensions.EntityFrameworkCore.Abstractions;

namespace EasyID.Server.Database.Models
{
    [Table("app_access_groups")]
    public class AppAccessGroup : BaseEntity<Guid>
    {
        [Column("user_id")]
        public Guid GroupId { get; set; }

        [Column("application_id")]
        public Guid ApplicationId { get; set; }

        public virtual Group Group { get; set; } = null!;
        public virtual Application Application { get; set; } = null!;
    }
}
