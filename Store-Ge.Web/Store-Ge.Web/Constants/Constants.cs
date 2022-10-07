namespace Store_Ge.Web.Constants
{
    public static class Constants
    {
        public static class Accounts
        {
            public static class Routes
            {
                public const string ACCOUNTS_CONTROLLER_ROUTE = "api/accounts";
                public const string ACCOUNTS_LOGIN_ENDPOINT_ROUTE = "login";
                public const string ACCOUNTS_REGISTER_ENDPOINT_ROUTE = "register";
            }

            public static class Shared
            {
                public const string CONFIRM_PASSWORD_STRING_LITERAL = "Confirm Password";
                public const string CONFIRM_PASSWORD_SHOULD_MATCH_THE_PASSWORD = "Confirm Password input should match the Password.";
            }
        }
    }
}
