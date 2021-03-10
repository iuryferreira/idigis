namespace External.Server.Contracts
{
    public static class ApiRoutes
    {
        private const string Base = "api/";

        public static class Index
        {
            public const string Signup = Base + "signup";
            public const string Signin = Base + "login";
        }
    }
}
