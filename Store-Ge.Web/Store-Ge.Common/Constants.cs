namespace Store_Ge.Common
{
    public static class Constants
    {
        public static class CommonConstants
        {
            public const string CREATED_ON_PROPERTY_STRING_LITERAL = "CreatedOn";
            public const string MODIFIED_ON_PROPERTY_STRING_LITERAL = "ModifiedOn";
        }

        public static class ValidationConstants
        {
            public const string PASSWORD_VALIDATION_PATTERN = @"^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{8,}$";
            public const int PASSWORD_MIN_LENGTH = 8;
            public const int EMAIL_MAX_LENGTH = 50;
            public const int STORE_NAME_MAX_LENGTH = 20;
            public const int PRODUCT_NAME_MAX_LENGTH = 50;
            public const int SUPPLIER_NAME_MAX_LENGTH = 20;
            public const int MAX_FAILED_LOGIN_ATTEMPTS = 3;
            public const int LOCKOUT_TIMESPAN_IN_MINUTES = 10;
        }

        public static class AccountsConstants
        {
            public const string USER_NOT_FOUND = "User not found";
            public const string EMAIL_NOT_CONFIRMED = "The provided email address is not confirmed.";
            public const string WRONG_CREDENTIALS = "The provided credentials are not correct.";
        } 
    }
}
