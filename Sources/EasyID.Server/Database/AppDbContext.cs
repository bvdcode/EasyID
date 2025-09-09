using Microsoft.EntityFrameworkCore;
using EasyExtensions.EntityFrameworkCore.Database;

namespace EasyID.Server.Database
{
    public class AppDbContext(DbContextOptions options) : AuditedDbContext(options)
    {

    }
}