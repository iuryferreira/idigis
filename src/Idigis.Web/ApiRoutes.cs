namespace Idigis.Web
{
    public static class ApiRoutes
    {
        public static class Church
        {
            public const string Signin = "/api/signin";
            public const string Signup = "/api/signup";
        }

        public static class Offer
        {
            public const string Add = "/api/offers";
            public const string List = "/api/offers";
            public const string Delete = "/api/offers";
            public const string Edit = "/api/offers";
        }

        public static class Member
        {
            public const string List = "/api/members";
            public const string Add = "/api/members";
            public const string Delete = "/api/members";
            public const string Edit = "/api/members";
        }

        public static class Tithe
        {
            public const string Add = "/api/tithes";
            public const string List = "/api/tithes";
            public const string Delete = "/api/tithes";
            public const string Edit = "/api/tithes";
        }
    }
}
