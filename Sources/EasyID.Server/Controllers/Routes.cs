namespace EasyID.Server.Controllers
{
    public static class Routes
    {
        public const string Base = "/api";
        public const string Version = Base + "/v1";

        public const string Auth = Version + "/auth";
        public const string Users = Version + "/users";
        public const string Metrics = Version + "/metrics";
    }
}
