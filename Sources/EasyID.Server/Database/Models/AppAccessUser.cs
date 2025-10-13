using System.ComponentModel.DataAnnotations.Schema;
using EasyExtensions.EntityFrameworkCore.Abstractions;

namespace EasyID.Server.Database.Models
{
    [Table("app_access_users")]
    public class AppAccessUser : BaseEntity<Guid>
    {
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("application_id")]
        public Guid ApplicationId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Application Application { get; set; } = null!;
    }
}
