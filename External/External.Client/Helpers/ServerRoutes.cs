namespace External.Client.Helpers
{
    public static class ServerRoutes
    {
        private const string Base = "api/";

        public static class Church
        {
            public const string Signup = Base + "signup";
            public const string Signin = Base + "login";
        }
    }
}
