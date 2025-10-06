namespace EasyID.Server
{
    public static class Constants
    {
        public const string AppName = "EasyID";

        public static class SystemGroups
        {
            public const string Users = "easyid-users";
            public const string Admin = "easyid-administrators";
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
                public const string View = "users.view";
                public const string Edit = "users.edit";
                public const string Delete = "users.delete";
                public const string Create = "users.create";
                public const string ChangeAvatar = "users.change_avatar";
                public const string ChangePassword = "users.change_password";
            }
        }
    }
}
