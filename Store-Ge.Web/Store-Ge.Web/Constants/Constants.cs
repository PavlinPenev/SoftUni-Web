namespace Store_Ge.Web.Constants
{
    public static class Constants
    {
        public static class Accounts
        {
            public static class Routes
            {
                public const string ACCOUNTS_CONTROLLER = "api/accounts";
                public const string ACCOUNTS_LOGIN_ENDPOINT = "login";
                public const string ACCOUNTS_REGISTER_ENDPOINT = "register";
                public const string REFRESH_ACCESS_TOKEN_ENDPOINT = "refresh-token";
                public const string CONFIRM_EMAIL_ENDPOINT = "confirm-email";
                public const string RESEND_EMAIL_ENDPOINT = "resend-confirmation-email";
            }

            public static class Shared
            {
                public const string CONFIRM_PASSWORD_STRING_LITERAL = "Confirm Password";
                public const string CONFIRM_PASSWORD_SHOULD_MATCH_THE_PASSWORD = "Confirm Password input should match the Password.";
            }
        }
    }
}
