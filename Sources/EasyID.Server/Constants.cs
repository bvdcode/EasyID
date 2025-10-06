namespace EasyID.Server
{
    public static class Constants
    {
        public const string AppName = "EasyID";

        public static class SystemGroups
        {
            public const string Users = "builtin.users";
            public const string Admin = "builtin.administrators";
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
                public const string List = "easyid.users.list";
                public const string Read = "easyid.users.read";
                public const string Create = "easyid.users.create";
                public const string Update = "easyid.users.update";
                public const string Delete = "easyid.users.delete";

                public const string AvatarUpdate = "easyid.users.avatar.update";
                public const string PasswordUpdate = "easyid.users.password.update";
                public const string PasswordReset = "easyid.users.password.reset";
                public const string PasswordForce = "easyid.users.password.force_change";
            }
        }
    }
}
