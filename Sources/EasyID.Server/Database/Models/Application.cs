using EasyID.Server.Database.Models.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyID.Server.Database.Models
{
    [Table("applications")]
    public class Application : BaseIdentityEntity
    {
        [Column("image_webp_bytes")]
        public byte[]? ImageWebPBytes { get; set; }
    }
}
