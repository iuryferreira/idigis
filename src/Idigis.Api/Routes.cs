namespace Idigis.Api
{
    public static class Routes
    {
        public static class Index
        {
            public const string Base = "api/";
            public const string Signup = Base + "signup";
        }

        public static class Church
        {
            public const string Base = "api/churches/";
        }

        public static class Offer
        {
            public const string Base = "api/offers";
        }

        public static class Member
        {
            public const string Base = "api/members";
        }
    }
}
