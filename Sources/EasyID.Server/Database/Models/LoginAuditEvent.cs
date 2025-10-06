using System.Net;
using System.ComponentModel.DataAnnotations.Schema;
using EasyExtensions.EntityFrameworkCore.Abstractions;

namespace EasyID.Server.Database.Models
{
    [Table("login_audit_events")]
    public class LoginAuditEvent : BaseEntity<Guid>
    {
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("ip_address")]
        public IPAddress IPAddress { get; set; } = IPAddress.None;
    }
}
