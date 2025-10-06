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

        [Column("is_successful")]
        public bool IsSuccessful { get; set; }

        [Column("reason")]
        public string? Reason { get; set; }

        [Column("user_agent")]
        public string UserAgent { get; set; } = null!;
    }
}
