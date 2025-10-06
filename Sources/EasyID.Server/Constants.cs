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
        }
    }
}
