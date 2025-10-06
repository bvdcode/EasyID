namespace EasyID.Server
{
    public static class Constants
    {
        public const string AppName = "EasyID";

        public static class SystemGroups
        {
            public const string Users = "builtin.users";
            public const string Admins = "builtin.admins";
        }

        public static class SystemRoles
        {
            public const string Admin = "easyid.admin";
            public const string User = "easyid.user";
        }

        public static class SystemPermissions
        {
            public static class Users
            {
                public const string View = "easyid.users.read";
                public const string Edit = "easyid.users.update";
                public const string Delete = "easyid.users.delete";
                public const string Create = "easyid.users.create";
                public const string ChangeAvatar = "easyid.users.change_avatar";
                public const string ChangePassword = "easyid.users.change_password";
            }

            public static class Apps
            {
                public const string View = "easyid.apps.read";
                public const string Create = "easyid.apps.create";
                public const string Update = "easyid.apps.update";
                public const string Delete = "easyid.apps.delete";
            }

            public static class Flags
            {
                public const string View = "easyid.flags.read";
                public const string Create = "easyid.flags.create";
                public const string Update = "easyid.flags.update";
                public const string Delete = "easyid.flags.delete";
            }

            public static class Groups
            {
                public const string View = "easyid.groups.read";
                public const string Create = "easyid.groups.create";
                public const string Update = "easyid.groups.update";
                public const string Delete = "easyid.groups.delete";
                public const string ManageMembers = "easyid.groups.manage_members";
            }

            public static class Permissions
            {
                public const string View = "easyid.permissions.read";
                public const string Create = "easyid.permissions.create";
                public const string Update = "easyid.permissions.update";
                public const string Delete = "easyid.permissions.delete";
                public const string Assign = "easyid.permissions.assign";
            }
        }
    }
}
