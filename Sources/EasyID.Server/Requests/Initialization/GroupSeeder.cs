using EasyID.Server.Database;
using EasyID.Server.Database.Models;

namespace EasyID.Server.Requests.Initialization
{
    internal static class GroupSeeder
    {
        public static async Task<IReadOnlyList<Group>> SeedSystemGroupsAsync(
            AppDbContext dbContext,
            User initialUser,
            CancellationToken cancellationToken)
        {
            var adminGroup = new Group
            {
                IsSystem = true,
                Name = Constants.SystemGroups.Admins,
                DisplayName = Constants.AppName + " Administrators",
                Description = "System administrators group with full access to administration features.",
            };

            var usersGroup = new Group
            {
                IsSystem = true,
                Name = Constants.SystemGroups.Users,
                DisplayName = Constants.AppName + " Users",
                Description = "Default users group with limited permissions.",
            };

            await dbContext.Groups.AddRangeAsync([adminGroup, usersGroup], cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            await AddUserToGroupsAsync(dbContext, initialUser, adminGroup, usersGroup, cancellationToken);

            return [adminGroup, usersGroup];
        }

        private static async Task AddUserToGroupsAsync(
            AppDbContext dbContext,
            User user,
            Group adminGroup,
            Group usersGroup,
            CancellationToken cancellationToken)
        {
            await dbContext.GroupUsers.AddRangeAsync(
            [
                new GroupUser { GroupId = adminGroup.Id, UserId = user.Id },
                new GroupUser { GroupId = usersGroup.Id, UserId = user.Id }
            ], cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
