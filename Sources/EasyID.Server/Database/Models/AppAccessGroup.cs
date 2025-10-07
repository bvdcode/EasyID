using System.ComponentModel.DataAnnotations.Schema;
using EasyExtensions.EntityFrameworkCore.Abstractions;

namespace EasyID.Server.Database.Models
{
    [Table("app_access_groups")]
    public class AppAccessGroup : BaseEntity<Guid>
    {

    }
}
