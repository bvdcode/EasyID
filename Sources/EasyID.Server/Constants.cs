namespace EasyID.Server
{
    public static class Constants
    {
        public const string AppName = "EasyID";

        public static class SystemGroups
        {
            public const string Users = "builtin-users";
            public const string Admin = "builtin-administrators";
        }

        public static class SystemRoles
        {
            public const string Admin = "admin";
            public const string User = "user";
        }

        public static class SystemPermissions
        {
            public static class Users
            {
                public const string View = "builtin.users.read";
                public const string Edit = "builtin.users.update";
                public const string Delete = "builtin.users.delete";
                public const string Create = "builtin.users.create";
                public const string ChangeAvatar = "builtin.users.change_avatar";
                public const string ChangePassword = "builtin.users.change_password";
            }

            public static class Apps
            {
                public const string View = "builtin.apps.read";
                public const string Create = "builtin.apps.create";
                public const string Update = "builtin.apps.update";
                public const string Delete = "builtin.apps.delete";
            }

            public static class Flags
            {
                public const string View = "builtin.flags.read";
                public const string Create = "builtin.flags.create";
                public const string Update = "builtin.flags.update";
                public const string Delete = "builtin.flags.delete";
            }

            public static class Groups
            {
                public const string View = "builtin.groups.read";
                public const string Create = "builtin.groups.create";
                public const string Update = "builtin.groups.update";
                public const string Delete = "builtin.groups.delete";
                public const string ManageMembers = "builtin.groups.manage_members";
            }

            public static class Permissions
            {
                public const string View = "builtin.permissions.read";
                public const string Create = "builtin.permissions.create";
                public const string Update = "builtin.permissions.update";
                public const string Delete = "builtin.permissions.delete";
                public const string Assign = "builtin.permissions.assign";
            }
        }
    }
}
